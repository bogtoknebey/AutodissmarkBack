namespace Autodissmark.Core.Helpers;

public static class SyntaxHelper
{
    public static List<string> GetTextWordsInLowercase(string text)
    {
        var textWords = text.Split(
            '\n', ' ', '"', '\'',
            '!', '-', '(', ')',
            '*', '.', '?', ']',
            '[', ',', ':', ';',
            '\r', '\t', '0', '1',
            '2', '3', '4', '5',
            '6', '7', '8', '9'
        )
        .Select(w => w.ToLower())
        .ToList();

        return textWords;
    }
}
