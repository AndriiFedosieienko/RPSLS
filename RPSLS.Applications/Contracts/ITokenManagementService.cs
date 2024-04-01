using RPSLS.Domain.Entities;

namespace RPSLS.Applications.Contracts
{
	public interface ITokenManagementService
	{
		/// <summary>
		///     Generates an authentication JWT for the specified user.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="tokenShelfLife"></param>
		/// <returns></returns>
		Task<string> GenerateJwt(EnhancedUser user, int? tokenShelfLife = null);

		/// <summary>
		///     Generates a refresh token for use in generating an authentication JWT.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<string> GenerateRefreshToken(EnhancedUser user);

		/// <summary>
		///     Uses a refresh token to generate a new JWT
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <param name="userName"></param>
		/// <returns></returns>
		Task<string> RefreshJwt(string refreshToken, string userName);
	}
}
