using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels.Diss;
using AutoMapper;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;

public class DissWriteRepository : IDissWriteRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public DissWriteRepository(ApplicationDataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<int> Create(DissModel model, CancellationToken ct = default)
    {
        // TODO: fix: it isn't change DissAcapella wit=h Diss Entity
        var entity = _mapper.Map<DissEntity>(model);
        await _context.Disses.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(int id, CancellationToken ct = default)
    {
        // TODO: check if DissAcapellas will be deleted also
        var entity = await _context.Disses.FindAsync(id, ct);

        if (entity == null)
        {
            throw new ArgumentException($"Diss with id {id} not found");
        }

        _context.Disses.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}
