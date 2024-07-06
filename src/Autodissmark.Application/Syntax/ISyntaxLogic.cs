namespace Autodissmark.Application.Syntax;

public interface ISyntaxLogic
{
    Task<Dictionary<string, int>> GetIncludes(int authorId, int minimalLength, CancellationToken ct);
    Task<ICollection<string>> GetAuthorRandomWords(int authorId, int minimalLength, int wordsCount, CancellationToken ct);
}
