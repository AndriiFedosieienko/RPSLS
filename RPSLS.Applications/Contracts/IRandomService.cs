namespace RPSLS.Applications.Contracts
{
    public interface IRandomService
    {
        Task<int> GenerateRandomNumberAsync(CancellationToken cancellationToken);
    }
}