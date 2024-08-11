using Autodissmark.Application.Diss.DTO;

namespace Autodissmark.Application.Diss;

public interface IDissLogic
{
    Task<int> CreateDiss(CreateDissDTO dto, CancellationToken ct);
    Task<GetDissDTO> GetDiss(int id, CancellationToken ct);
    Task<ICollection<GetDissDTO>> GetAllDisses(int textId, CancellationToken ct);
    Task<ICollection<GetDissDTO>> GetDissesPage(int textId, int pageSize, int pageNumber, CancellationToken ct);
    Task DeleteDiss(int id, CancellationToken ct);
}
