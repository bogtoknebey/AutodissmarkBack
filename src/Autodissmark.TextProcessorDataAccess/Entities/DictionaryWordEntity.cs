namespace Autodissmark.TextProcessorDataAccess.Entities;

public class DictionaryWordEntity
{
    public int Id { get; set; }
    public int DictionaryEntityId { get; set; }
    public string Word { get; set; }

    public virtual DictionaryEntity DictionaryEntity { get; set; }
}
