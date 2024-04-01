using Microsoft.EntityFrameworkCore;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Persistence.Contexts;

namespace RPSLS.Applications.Services.Repositories
{
    public class RulesRepository : IRulesRepository
	{
		private readonly RPSLSDbContext _dbContext;

		public RulesRepository(RPSLSDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<bool> IsPlayerWinAsync(int playerChoiceId, int opponentChoiceId, CancellationToken cancellationToken)
		{
			return await _dbContext.Rules.AnyAsync(x => x.WinnerChoiceId == playerChoiceId && x.LooserChoiceId == opponentChoiceId,
				cancellationToken);
		}
	}
}
