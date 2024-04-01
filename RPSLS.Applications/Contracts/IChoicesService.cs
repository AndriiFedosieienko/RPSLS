using RPSLS.Applications.Models;

namespace RPSLS.Applications.Contracts
{
    public interface IChoicesService
    {
        Task<IEnumerable<ChoiceModel>> GetAllChoicesAsync(CancellationToken cancellationToken);
        Task<ChoiceModel?> GetSingeChoice(CancellationToken cancellationToken);
    }
}