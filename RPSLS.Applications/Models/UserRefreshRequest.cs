using System.ComponentModel.DataAnnotations;

namespace RPSLS.Applications.Models
{
    public class UserRefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
