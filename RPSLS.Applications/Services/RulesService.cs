using RPSLS.Applications.Contracts;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Models;

namespace RPSLS.Applications.Services
{
    public class RulesService : IRulesService
	{
		private readonly IRulesRepository _rulesRepository;
		private readonly IRandomService _randomService;

		public RulesService(IRulesRepository rulesRepository,
			IRandomService randomService)
		{
			_rulesRepository = rulesRepository;
			_randomService = randomService;
		}

		public async Task<GameResultsModel> GetGameResultsAsync(int playerChoiceId, CancellationToken cancellationToken)
		{
			var result = new GameResultsModel
			{
				Player = playerChoiceId
			};
			var randomNumber = await _randomService.GenerateRandomNumberAsync(cancellationToken);
			var randomId = randomNumber % 5;
			if (randomId == 0)
			{
				randomId = 5;
			}
			result.Bot = randomId;
			if (playerChoiceId == randomId)
			{
				result.Results = "Tie";
			}
			else
			{
				var isWin = await _rulesRepository.IsPlayerWinAsync(playerChoiceId, randomId, cancellationToken);
				result.Results = isWin ? "Win" : "Lose";
			}

			return result;
		}
	}
}
