namespace RPSLS.Applications.Models
{
    /// <summary>
    /// Allows the user to refresh their access credentials without having to login again.
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Denotes who the refresh token is for.  Only the person generating the refresh token can use this token.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Denotes how long the refresh token can be used for.
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// If the user is impersonating, this value holds the actual value of the person impersonating.
        /// </summary>
        public string ActualUserName { get; set; } = string.Empty;
    }
}
