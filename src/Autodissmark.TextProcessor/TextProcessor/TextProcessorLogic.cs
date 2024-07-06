using Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;
using Core.Extentions;
using System.Collections.Generic;
using System.Text;

namespace Autodissmark.TextProcessor.TextProcessor;

public class TextProcessorLogic : ITextProcessorLogic
{
    private const string GlobalDictionaryName = "global";
    private const string LocalDictionaryName = "local";

    private readonly IDictionaryReadRepository _dictionaryReadRepository;
    private readonly IDictionaryWordReadRepository _dictionaryWordReadRepository;

    public TextProcessorLogic(
        IDictionaryReadRepository dictionaryReadRepository, 
        IDictionaryWordReadRepository dictionaryWordReadRepository)
    {
        _dictionaryReadRepository = dictionaryReadRepository;
        _dictionaryWordReadRepository = dictionaryWordReadRepository;
    }

    public string AddTargetToText(string text, string target, CancellationToken ct)
    {
        StringBuilder result = new StringBuilder();
        Random random = new Random();

        var lines = text.Split('\n');
        string[] lineWords;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "")
            {
                continue;
            }

            lineWords = lines[i].Split(' ', ',', '.', ':', ';', '!', '?');
            var replaceWordIndex = random.Next(0, lineWords.Length);
            lineWords[replaceWordIndex] = target;

            foreach (var word in lineWords)
            {
                result.Append($"{word} ");
            }
            result.Append('\n');
        }

        return result.ToString();
    }

    public string ExpandText(string text, CancellationToken ct)
    {
        return $"{text}\n{text}";
    }

    public async Task<ICollection<string>> GetRandomWords(int dictionaryId, int wordsCount, CancellationToken ct)
    {
        var words = await _dictionaryWordReadRepository.GetRandomWords(dictionaryId, wordsCount, ct);
        return words;
    }

    public async Task<string> GenerateRandomText(int linesCount, int wordsInLineCount, CancellationToken ct)
    {
        var globalDictionaryId = await _dictionaryReadRepository.GetFirstIdByName(GlobalDictionaryName, ct);
        var localDictionaryId = await _dictionaryReadRepository.GetFirstIdByName(LocalDictionaryName, ct);

        var totalWordsCount = linesCount * wordsInLineCount;
        var globalWordsCount = totalWordsCount / 2;
        var localWordsCount = totalWordsCount - globalWordsCount;

        var globalWords = await _dictionaryWordReadRepository.GetRandomWords(globalDictionaryId, globalWordsCount, ct);
        var localWords = await _dictionaryWordReadRepository.GetRandomWords(localDictionaryId, localWordsCount, ct);

        string[] words = new string[totalWordsCount];

        // odd words
        int globalInd = 1;
        globalWords.ToList().ForEach(word => {
            words[globalInd] = word;
            globalInd += 2;
        });

        // even words
        int localInd = 0;
        localWords.ToList().ForEach(word => {
            words[localInd] = word;
            localInd += 2;
        });

        StringBuilder resultText = new StringBuilder();

        int wordInd = 0;
        for (int i = 0; i < linesCount; i++)
        {
            for (int j = 0; j < wordsInLineCount; j++)
            {
                string word = words[wordInd];
                
                // first word in line
                if (j == 0)
                {
                    word = word.UppercaseFirstLetter();
                }

                // not last word in line
                if (j != wordsInLineCount - 1)
                {
                    word += " ";
                }

                resultText.Append(word);
                wordInd++;
            }
            resultText.Append("\n");
        }

        return resultText.ToString();
    }
}
