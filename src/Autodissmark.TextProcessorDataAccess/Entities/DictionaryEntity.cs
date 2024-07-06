namespace Autodissmark.TextProcessorDataAccess.Entities;

public class DictionaryEntity
{
    public DictionaryEntity()
    {
        DictionaryWordEntities = new List<DictionaryWordEntity>();
        TextBaseDicitonaryEntities = new List<TextBaseDictionaryEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<DictionaryWordEntity> DictionaryWordEntities { get; set; }
    public virtual ICollection<TextBaseDictionaryEntity> TextBaseDicitonaryEntities { get; set; }
}
