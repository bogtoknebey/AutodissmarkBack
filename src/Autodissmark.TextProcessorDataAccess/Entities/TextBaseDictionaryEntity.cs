namespace Autodissmark.TextProcessorDataAccess.Entities;

public class TextBaseDictionaryEntity
{
    public int Id { get; set; }
    public int TextBaseEntityId { get; set; }
    public int DictionaryEntityId { get; set; }

    public virtual TextBaseEntity TextBaseEntity { get; set; }
    public virtual DictionaryEntity DictionaryEntity { get; set; }
}