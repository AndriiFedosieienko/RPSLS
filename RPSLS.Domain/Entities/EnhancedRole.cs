using Microsoft.AspNetCore.Identity;

namespace RPSLS.Domain.Entities
{
    /// <summary>
    /// Represents an role.
    /// </summary>
    public class EnhancedRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Represents the role description.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
