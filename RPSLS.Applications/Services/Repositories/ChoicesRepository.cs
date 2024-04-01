using Microsoft.EntityFrameworkCore;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Models;
using RPSLS.Persistence.Contexts;

namespace RPSLS.Applications.Services.Repositories
{
    public class ChoicesRepository : IChoicesRepository
	{
		private readonly RPSLSDbContext _dbContext;

		public ChoicesRepository(RPSLSDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<ChoiceModel>> GetAllChoicesAsync(CancellationToken cancellationToken)
		{
			return await _dbContext.Choices.Select(x => new ChoiceModel
			{
				Id = x.Id,
				Name = x.Name
			}).ToListAsync(cancellationToken);
		}

		public async Task<ChoiceModel?> GetSingeChoiceAsync(int id)
		{
			var choice = await _dbContext.Choices.FindAsync(id);
			return choice != null 
				? new ChoiceModel
				{
					Id = choice.Id,
					Name = choice.Name
				}
				: null;
		}
	}
}
