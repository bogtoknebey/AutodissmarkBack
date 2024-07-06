using Autodissmark.Domain.Enums;

namespace Autodissmark.Domain.ApplicationModels;

public class AuthorModel()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public static AuthorModel Create(string name, string email, string password, Role role)
    {
        return new AuthorModel()
        {
            Name = name,
            Email = email,
            Password = password,
            Role = role
        };
    }
}