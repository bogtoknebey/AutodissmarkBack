namespace Autodissmark.TextProcessor.TextProcessor;

public interface ITextProcessorLogic
{
    string ExpandText(string text, CancellationToken ct);
    string AddTargetToText(string text, string target, CancellationToken ct);
    Task<ICollection<string>> GetRandomWords(int dictionaryId, int wordsCount, CancellationToken ct);
    Task<string> GenerateRandomText(int linesCount, int wordsInLineCount, CancellationToken ct);
}
