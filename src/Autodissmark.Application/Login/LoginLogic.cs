using Autodissmark.Application.Login.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

namespace Autodissmark.Application.Login;

public class LoginLogic : ILoginLogic
{
    private readonly IAuthorReadRepository _readRepository;

    public LoginLogic(
        IAuthorReadRepository readRepository
    )
    {
        _readRepository = readRepository;
    }

    public async Task<LoginOutputDTO> Login(LoginInputDTO dto, CancellationToken ct)
    {
        var author = await _readRepository.GetByEmail(dto.Email, ct);

        if (author == null)
        {
            throw new Exception("Login rejected. Wrong email.");
        }

        if (author.Password != dto.Password)
        {
            throw new Exception("Login rejected. Wrong password.");
        }

        var response = new LoginOutputDTO(author.Id, author.Role);
        return response;
    }
}
