namespace Autodissmark.Domain.TextProcessorModels;

public class DictionaryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Words { get; set; }

    public static DictionaryModel Create(string name, List<string> words)
    {
        return new DictionaryModel()
        {
            Name = name,
            Words = words
        };
    }
}
