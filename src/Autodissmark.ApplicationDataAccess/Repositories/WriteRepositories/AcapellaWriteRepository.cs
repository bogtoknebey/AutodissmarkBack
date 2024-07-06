using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;

public class AcapellaWriteRepository : IAcapellaWriteRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public AcapellaWriteRepository(ApplicationDataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<int> Create(AcapellaModel model, CancellationToken ct = default)
    {
        var entity = _mapper.Map<AcapellaEntity>(model);
        await _context.Acapellas.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Delete(int id, CancellationToken ct = default)
    {
        var entity = await _context.Acapellas.FindAsync(id, ct);

        if (entity == null)
        {
            throw new ArgumentException($"Acapella with id:{id} not found");
        }

        _context.Acapellas.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}
