using Newtonsoft.Json;

namespace RPSLS.Applications.Models
{
    /// <summary>
    /// Response including an authentication token and a refresh token
    /// </summary>
    public class AuthenticationResponse
    {
        [JsonProperty("auth_token")]
        public string AuthenticationToken { get; set; } = string.Empty;

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
