using Autodissmark.Domain.TextProcessorModels;
using Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories;

public class DictionaryReadRepository : IDictionaryReadRepository
{
    private readonly TextProcessorDataContext _context;
    private readonly IMapper _mapper;

    public DictionaryReadRepository(TextProcessorDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DictionaryModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Dictionaries
                                .Include(d => d.DictionaryWordEntities)
                                .FirstOrDefaultAsync(d => d.Id == id);

        var model = _mapper.Map<DictionaryModel>(entity);
        model.Words = entity.DictionaryWordEntities.Select(dw => dw.Word).ToList();

        return model;
    }

    public async Task<int> GetFirstIdByName(string name, CancellationToken ct = default)
    {
        var entity = await _context.Dictionaries.FirstOrDefaultAsync(d => d.Name == name);
        return entity.Id;
    }
}
