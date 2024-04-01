using RPSLS.Domain.Entities;

namespace RPSLS.Applications.Contracts.Repositories
{
	public interface IUsersRepository
	{
		Task<EnhancedUser?> GetUserByName(string userName);
	}
}