using Microsoft.EntityFrameworkCore;
using RPSLS.Domain.Entities;

namespace RPSLS.Persistence.Contexts
{
	public class RPSLSDbContext : DbContext
	{
		public RPSLSDbContext(DbContextOptions<RPSLSDbContext> options) : base(options)
		{
		}

		public DbSet<Choice> Choices { get; set; }

		public DbSet<Rule> Rules { get; set; }
	}
}
