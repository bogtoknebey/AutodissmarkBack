namespace Autodissmark.TextProcessorDataAccess.Entities;

public class ArtistEntity
{
    public ArtistEntity()
    {
        ArtistTextEntities = new List<ArtistTextEntity>();
        TextBaseArtistEntities = new List<TextBaseArtistEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    
    public virtual ICollection<ArtistTextEntity> ArtistTextEntities { get; set; }
    public virtual ICollection<TextBaseArtistEntity> TextBaseArtistEntities { get; set; }
}
