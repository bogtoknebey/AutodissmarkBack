using Autodissmark.Domain.Enums;

namespace Autodissmark.TextProcessorDataAccess.Entities;

public class ArtistTextEntity
{
    public int Id { get; set; }
    public int ArtistEntityId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }

    public virtual ArtistEntity ArtistEntity { get; set; }
}
