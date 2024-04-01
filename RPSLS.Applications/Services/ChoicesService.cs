using RPSLS.Applications.Contracts;
using RPSLS.Applications.Contracts.Repositories;
using RPSLS.Applications.Models;

namespace RPSLS.Applications.Services
{
	public class ChoicesService : IChoicesService
	{
		private readonly IChoicesRepository _choicesRepository;
		private readonly IRandomService _randomService;

		public ChoicesService(IChoicesRepository choicesRepository,
			IRandomService randomService)
		{
			_choicesRepository = choicesRepository;
			_randomService = randomService;
		}

		public async Task<IEnumerable<ChoiceModel>> GetAllChoicesAsync(CancellationToken cancellationToken)
		{
			return await _choicesRepository.GetAllChoicesAsync(cancellationToken);
		}

		public async Task<ChoiceModel?> GetSingeChoice(CancellationToken cancellationToken)
		{
			var randomNumber = await _randomService.GenerateRandomNumberAsync(cancellationToken);
			var randomId = randomNumber % 5;
			if (randomId == 0)
			{
				randomId = 5;
			}
			return await _choicesRepository.GetSingeChoiceAsync(randomId);
		}
	}
}
