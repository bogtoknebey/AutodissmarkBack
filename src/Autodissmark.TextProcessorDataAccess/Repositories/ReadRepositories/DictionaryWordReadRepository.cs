using Autodissmark.TextProcessorDataAccess.Entities;
using Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories;

public class DictionaryWordReadRepository : IDictionaryWordReadRepository
{
    private readonly TextProcessorDataContext _context;
    private readonly IMapper _mapper;

    public DictionaryWordReadRepository(TextProcessorDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ICollection<string>> GetRandomWords(int dictionaryId, int wordsCount, CancellationToken ct = default)
    {
        var sql = $"SELECT TOP({wordsCount}) Word " +
                  $"FROM DictionaryWords " +
                  $"Where DictionaryEntityId = {dictionaryId} " +
                  $"ORDER BY NEWID()";

        var randomWords = await _context.DictionaryWords
                                          .FromSqlRaw(sql)
                                          .Select(dw => dw.Word)
                                          .ToListAsync(ct);

        return randomWords;
    }
}
