using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RPSLS.Applications.Contracts;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Models;
using RPSLS.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RPSLS.Applications.Services
{
	public class TokenManagementService : ITokenManagementService
	{
		private readonly IConfiguration _config;
		private readonly IUsersRepository _usersRepository;
		private readonly IEncryptionService _encryptionService;
		private readonly UserManager<EnhancedUser> _userManager;

		public TokenManagementService(IConfiguration config, IUsersRepository usersRepository,
			IEncryptionService encryptionService, UserManager<EnhancedUser> userManager)
		{
			_config = config;
			_usersRepository = usersRepository;
			_encryptionService = encryptionService;
			_userManager = userManager;
		}

		public async Task<string> GenerateJwt(EnhancedUser user,
			int? tokenShelfLife = null)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email.ToLower()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().ToLower()),
				new Claim("uid", user.Id.ToString().ToLower()),
				new Claim("unm", user.UserName.ToLower()),
				new Claim("fnm", user.FirstName.ToLower()),
				new Claim("lnm", user.LastName.ToLower()),
				new Claim("eml", user.Email.ToLower()),
				new Claim("dls", user.Dealership.ToLower())
			};

			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			return WriteToken(claims, tokenShelfLife);
		}

		public async Task<string> GenerateRefreshToken(EnhancedUser user)
		{
			var actualUserName = user.UserName;

			var expiresInMinutes = GetRefreshTokenDurationInMinutes();
			var token = new RefreshToken
			{
				Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
				UserName = user.UserName,
				ActualUserName = actualUserName
			};

			var payload = JsonConvert.SerializeObject(token);
			var encrypted = await _encryptionService.Encrypt(payload);

			return encrypted;
		}

		public async Task<string> RefreshJwt(string refreshToken, string userName)
		{
			var decryptedPayload = await _encryptionService.Decrypt(refreshToken);
			var decryptedRefreshToken = JsonConvert.DeserializeObject<RefreshToken>(decryptedPayload);

			var normalizedName = _userManager.NormalizeName(userName);
			var user = await _usersRepository.GetUserByName(normalizedName);
			if (user == null)
			{
				throw new ArgumentException(
					$"User '{userName}' cannot be found in the data store by name.");
			}

			if (decryptedRefreshToken == null)
			{
				throw new ArgumentException(
					"The encrypted payload passed could not be decrypted to a valid refresh token.");
			}

			if (!decryptedRefreshToken.UserName.Equals(user.UserName, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new ArgumentException(
					$"The refresh token is for '{decryptedRefreshToken.UserName}', but the user requesting it is '{user.UserName}'.");
			}

			if (decryptedRefreshToken.Expires < DateTime.UtcNow)
			{
				throw new ArgumentException("The refresh token is expired.");
			}

			string token;
			if (decryptedRefreshToken.UserName.Equals(decryptedRefreshToken.ActualUserName))
			{
				token = await GenerateJwt(user);
			}
			else
			{
				throw new ArgumentException("Username for token was changed.");
			}

			return token;
		}
		private string WriteToken(IEnumerable<Claim> claims, int? tokenShelfLife = null)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Security:Authentication:Tokens:Key"]));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var expirationDate = DateTime.UtcNow.AddMinutes(20);

			if (tokenShelfLife.HasValue)
			{
				expirationDate = DateTime.UtcNow.AddSeconds(tokenShelfLife.Value);
			}
			else if (int.TryParse(_config["Security:Authentication:Tokens:LastsForMinutes"], out int lastsForMinutes))
			{
				expirationDate = DateTime.UtcNow.AddMinutes(lastsForMinutes);
			}

			var token = new JwtSecurityToken(
				_config["Security:Authentication:Tokens:Issuer"],
				_config["Security:Authentication:Tokens:Audience"],
				claims,
				expires: expirationDate,
				signingCredentials: creds);

			var returnVal = new JwtSecurityTokenHandler().WriteToken(token);

			return returnVal;

		}

		private TimeSpan GetConfigurationTimeSpan(IConfiguration config, string key, TimeSpan defaultTimespan)
		{
			if (!int.TryParse(config[key], out int value))
			{
				return defaultTimespan;
			}

			return TimeSpan.FromMinutes(value);
		}

		private int GetRefreshTokenDurationInMinutes()
		{
			return (int)GetConfigurationTimeSpan(_config,
				"Security:Authentication:Tokens:RefreshTokenLastsForMinutes",
				TimeSpan.FromDays(30)).TotalMinutes;
		}
	}
}
