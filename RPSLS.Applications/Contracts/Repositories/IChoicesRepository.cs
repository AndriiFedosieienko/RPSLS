using RPSLS.Applications.Models;

namespace RPSLS.Applications.Contracts.Repositories
{
    public interface IChoicesRepository
    {
        Task<IEnumerable<ChoiceModel>> GetAllChoicesAsync(CancellationToken cancellationToken);
        Task<ChoiceModel?> GetSingeChoiceAsync(int id);
    }
}