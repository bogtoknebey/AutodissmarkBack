using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class AcapellaReadRepository : IAcapellaReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public AcapellaReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AcapellaModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Acapellas.FirstOrDefaultAsync(e => e.Id == id, ct);
        return _mapper.Map<AcapellaModel>(entity);
    }

    public async Task<ICollection<AcapellaModel>> GetAllByTextId(int textId, CancellationToken ct = default)
    {
        var entities = await _context.Acapellas.Where(e => e.TextEntityId == textId).ToListAsync();
        var models = entities.Select(_mapper.Map<AcapellaModel>).ToList();
        return models;
    }

    public async Task<ICollection<AcapellaModel>> GetPageByTextId(int textId, int pageSize, int pageNumber, CancellationToken ct = default)
    {
        var skipAmount = (pageNumber - 1) * pageSize;

        var entities = await _context.Acapellas
            .Where(a => a.TextEntityId == textId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync(ct);

        return _mapper.Map<ICollection<AcapellaModel>>(entities);
    }

    public async Task<int> GetAuthorId(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
