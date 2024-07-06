using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IVoiceReadRepository
{
    Task<VoiceModel> GetById(int id, CancellationToken ct = default);
}
