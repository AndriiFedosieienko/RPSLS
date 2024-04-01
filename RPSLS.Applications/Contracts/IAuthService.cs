using RPSLS.Applications.Models;

namespace RPSLS.Applications.Contracts
{
    /// <summary>
    /// Serves as the operations responsible for authenticating a user.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Responsible for authenticating a user. Returns an auth
        /// response upon completion of authenticating the login.
        /// </summary>
        /// <param name="loginModel">Represents the login model to which is intended to be authenticated.</param>
        /// <returns>Returns a task that contains the auth response, which occurs upon completion of the supplied login model.</returns>
        Task<AuthenticationResponse> AuthenticateUser(LoginModel loginModel);
    }
}
