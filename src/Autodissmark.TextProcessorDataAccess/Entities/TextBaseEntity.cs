namespace Autodissmark.TextProcessorDataAccess.Entities;

public class TextBaseEntity
{
    public TextBaseEntity()
    {
        TextBaseDicitonaryEntities = new List<TextBaseDictionaryEntity>();
        TextBaseArtistEntities = new List<TextBaseArtistEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string ONNXURI { get; set; }

    public virtual ICollection<TextBaseArtistEntity> TextBaseArtistEntities { get; set; }
    public virtual ICollection<TextBaseDictionaryEntity> TextBaseDicitonaryEntities { get; set; }
}
