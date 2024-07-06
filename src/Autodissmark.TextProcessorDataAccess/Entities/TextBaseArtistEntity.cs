namespace Autodissmark.TextProcessorDataAccess.Entities;

public class TextBaseArtistEntity
{
    public int Id { get; set; }
    public int TextBaseEntityId { get; set; }
    public int ArtistEntityId { get; set; }

    public virtual TextBaseEntity TextBaseEntity { get; set; }
    public virtual ArtistEntity ArtistEntity { get; set; }
}
