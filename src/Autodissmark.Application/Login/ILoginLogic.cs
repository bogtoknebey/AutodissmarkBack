using Autodissmark.Application.Login.DTO;

namespace Autodissmark.Application.Login;

public interface ILoginLogic
{
    Task<LoginOutputDTO> Login(LoginInputDTO dto, CancellationToken ct);
}
