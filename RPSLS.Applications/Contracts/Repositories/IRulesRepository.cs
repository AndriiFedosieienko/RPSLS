namespace RPSLS.Applications.Contracts.Repositories
{
    public interface IRulesRepository
    {
        Task<bool> IsPlayerWinAsync(int playerChoiceId, int opponentChoiceId, CancellationToken cancellationToken);
    }
}