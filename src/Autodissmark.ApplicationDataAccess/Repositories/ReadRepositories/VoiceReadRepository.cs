using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class VoiceReadRepository : IVoiceReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public VoiceReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VoiceModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Voices.Where(v => v.Id == id)
                                          .Include(v => v.ArtistEntity)
                                          .FirstOrDefaultAsync(ct);
        return _mapper.Map<VoiceModel>(entity);
    }
}
