using Autodissmark.Domain.Enums;

namespace Autodissmark.ApplicationDataAccess.Entities;

public class AuthorEntity
{
    public AuthorEntity()
    {
        TextEntities = new List<TextEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public virtual ICollection<TextEntity> TextEntities { get; set; }
}
