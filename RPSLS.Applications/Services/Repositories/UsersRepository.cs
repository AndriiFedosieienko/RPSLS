using Microsoft.EntityFrameworkCore;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Domain.Entities;
using RPSLS.Persistence.Contexts;

namespace RPSLS.Applications.Services.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private readonly AuthDbContext _dbContext;

		public UsersRepository(AuthDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<EnhancedUser?> GetUserByName(string userName)
		{
			return await _dbContext.Users.FirstOrDefaultAsync(x =>
					x.UserName == userName);
		}
	}
}
