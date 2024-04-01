using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RPSLS.Domain.Entities
{
    /// <summary>
    /// Represents an role.
    /// </summary>
    public class EnhancedUser : IdentityUser<Guid>
    {
        [StringLength(35)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(35)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PreferredLanguage { get; set; } = string.Empty;

        [StringLength(50)]
        public string Dealership { get; set; } = string.Empty;
    }
}
