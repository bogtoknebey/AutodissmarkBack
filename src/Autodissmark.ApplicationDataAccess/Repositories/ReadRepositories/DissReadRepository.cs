using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels.Diss;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class DissReadRepository : IDissReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public DissReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DissModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Disses.FirstOrDefaultAsync(e => e.Id == id, ct);
        return _mapper.Map<DissModel>(entity);
    }

    public async Task<int> GetAuthorId(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
