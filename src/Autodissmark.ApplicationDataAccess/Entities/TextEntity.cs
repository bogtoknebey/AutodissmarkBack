namespace Autodissmark.ApplicationDataAccess.Entities;

public class TextEntity
{
    public TextEntity()
    {
        AcapellaEntities = new List<AcapellaEntity>();
    }

    public int Id { get; set; }
    public int AuthorEntityId { get; set; }
    public string Text { get; set; }
    public string Title { get; set; }
    public DateTime AddedDate { get; set; }

    public virtual AuthorEntity AuthorEntity { get; set; }
    public virtual ICollection<AcapellaEntity> AcapellaEntities { get; set; }
}