using RPSLS.Applications.Contracts;
using RPSLS.Domain.Entities;
using RPSLS.Applications.Exceptions;
using RPSLS.Applications.Models;
using Microsoft.AspNetCore.Identity;

namespace RPSLS.Applications.Services
{
    /// <summary>
    /// Serves as the service responsible for authentication.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<EnhancedUser> _userManager;
        private readonly SignInManager<EnhancedUser> _signInManager;
        private readonly ITokenManagementService _tokenManagementSvc;

        public AuthService(UserManager<EnhancedUser> userManager,
            SignInManager<EnhancedUser> signInManager,
            ITokenManagementService tokenManagementSvc) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenManagementSvc = tokenManagementSvc;
        }

        public async Task<AuthenticationResponse> AuthenticateUser(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            ValidateUser(user);
            return await SignInUser(user, loginModel);
        }

        private async Task<AuthenticationResponse> SignInUser(EnhancedUser user, LoginModel model)
        {
            var signedIn = await _signInManager.CheckPasswordSignInAsync(user, model?.Password, true);
            if (signedIn.Succeeded)
            {
                var result = await GetAuthResult(user);
                return result;
            }

            throw new NotAuthorizedException("Invalid username and/or password.");
        }

        private async Task<AuthenticationResponse> GetAuthResult(EnhancedUser user)
        {
            var authToken = await _tokenManagementSvc.GenerateJwt(user);
            var refreshToken = await _tokenManagementSvc.GenerateRefreshToken(user);

            return new AuthenticationResponse
            {
                AuthenticationToken = authToken,
                RefreshToken = refreshToken
            };
        }

        private void ValidateUser(EnhancedUser user)
        {
            if (user is null)
            {
                throw new NotAuthorizedException("Invalid username and/or password.");
            }
            if ((user.LockoutEnd ?? DateTimeOffset.MinValue) > DateTimeOffset.Now)
            {
                throw new NotAuthorizedException("User was locked out.");
            }
        }
    }
}
