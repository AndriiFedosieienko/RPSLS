using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RPSLS.Domain.Entities;

namespace RPSLS.Persistence.Contexts
{
	public class AuthDbContext : IdentityDbContext<EnhancedUser, EnhancedRole, Guid>
	{
		public AuthDbContext(
			DbContextOptions<AuthDbContext> options
		) : base(options)
		{
			Database.EnsureCreated();
		}
		protected override void OnModelCreating(
			ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<EnhancedUser>().Property(x => x.PreferredLanguage).HasDefaultValue("en");

			//Seed(builder);
		}

		private void Seed(ModelBuilder builder)
		{
			builder.Entity<EnhancedRole>().HasData(new EnhancedRole
			{
				Id = Guid.Parse("a04ccd60-13e6-4947-a8a9-23f9329da021"),
				Name = "Global Administrator",
				NormalizedName = "Global Administrator".ToUpper(),
				Description = "Allows to do all actions in the scope of the application."
			});
			PasswordHasher<EnhancedUser> passwordHasher = new PasswordHasher<EnhancedUser>();

			var user = new EnhancedUser
			{
				Id = Guid.Parse("a04ccd60-13e6-4977-a1a0-23f9329da018"),
				UserName = "Grand Master",
				NormalizedUserName = "Grand Master".ToUpper(),
				Email = "grandmaster@gmail.com",
				FirstName = "Grand",
				LastName = "Master",
				PreferredLanguage = "en",
				EmailConfirmed = true,
				SecurityStamp = "A18F441D-0833-44FD-85AF-B800831BF731"
			};
			user.PasswordHash = passwordHasher.HashPassword(user, "Pass1234#");
			builder.Entity<EnhancedUser>().HasData(user);
			builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
			{
				UserId = Guid.Parse("a04ccd60-13e6-4977-a1a0-23f9329da018"),
				RoleId = Guid.Parse("a04ccd60-13e6-4947-a8a9-23f9329da021")
			});
		}
	}
}
