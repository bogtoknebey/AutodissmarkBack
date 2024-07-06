namespace Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;

public interface IDictionaryWordReadRepository
{
    Task<ICollection<string>> GetRandomWords(int dictionaryId, int wordsCount, CancellationToken ct = default);
}
