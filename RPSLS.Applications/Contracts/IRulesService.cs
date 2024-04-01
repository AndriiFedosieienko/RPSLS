using RPSLS.Applications.Models;

namespace RPSLS.Applications.Contracts
{
    public interface IRulesService
    {
        Task<GameResultsModel> GetGameResultsAsync(int playerChoiceId, CancellationToken cancellationToken);
    }
}