namespace RPSLS.Applications.Models
{
    /// <summary>
    /// Serves as the login model that is used during authentication.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Represents the username of the login.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Represents the password field of the login.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
