using Autodissmark.Application.Diss.DTO;

namespace Autodissmark.Application.Diss;

public interface IDissLogic
{
    Task<int> CreateDiss(CreateDissDTO dto, CancellationToken ct);
    Task<GetDissDTO> GetDiss(int dissId, CancellationToken ct);
    Task DeleteDiss(int dissId, CancellationToken ct);
}
