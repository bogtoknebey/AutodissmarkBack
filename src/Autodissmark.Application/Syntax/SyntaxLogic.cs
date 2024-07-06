
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Core.Helpers;

namespace Autodissmark.Application.Syntax;

public class SyntaxLogic : ISyntaxLogic
{
    private readonly ITextReadRepository _textReadRepository;
    public SyntaxLogic(
        ITextReadRepository textReadRepository
    )
    {
        _textReadRepository = textReadRepository;
    }

    public async Task<Dictionary<string, int>> GetIncludes(int authorId, int minimalLength, CancellationToken ct)
    {
        Dictionary<string, int> includes = new Dictionary<string, int>();

        var textModels = await _textReadRepository.GetAllTexts(authorId, ct);

        foreach (var textModel in textModels)
        {
            var wordList = SyntaxHelper.GetTextWordsInLowercase(textModel.Text);

            foreach (var word in wordList)
            {
                if (word.Length < minimalLength)
                {
                    continue;
                }

                if (!includes.ContainsKey(word)) 
                {
                    includes[word] = 0;
                }

                includes[word]++;
            }
        }

        return includes;
    }

    public async Task<ICollection<string>> GetAuthorRandomWords(int authorId, int minimalLength, int wordsCount, CancellationToken ct)
    {
        List<string> randomWords = new List<string>();

        var includes = await GetIncludes(authorId, minimalLength, ct);

        if (includes is null) 
        {
            return null;
        }

        List<string> allWords = new List<string>(includes.Keys);

        Random rnd = new Random();
        int num;
        for (int i = 0; i < wordsCount; i++)
        {
            num = rnd.Next(0, allWords.Count);
            randomWords.Add(allWords[num]);
        }

        return randomWords;
    }
}
