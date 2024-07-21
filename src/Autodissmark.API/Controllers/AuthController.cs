using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Extentions;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.API.Services.JWTBuilder;
using Autodissmark.Application.Login;
using Autodissmark.Application.Login.DTO;
using Autodissmark.Domain.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJwtTokenBuilder _jwtTokenBuilder;
        private readonly ILoginLogic _loginLogic;

        public AuthController(
            IMapper mapper,
            IJwtTokenBuilder jwtTokenBuilder,
            ILoginLogic loginLogic
        )
        {
            _mapper = mapper;
            _jwtTokenBuilder = jwtTokenBuilder;
            _loginLogic = loginLogic;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            try
            {
                var dto = _mapper.Map<LoginInputDTO>(request);
                var loginResponse = await _loginLogic.Login(dto, ct);

                var jwtToken = _jwtTokenBuilder
                    .AddUserIdClaim(loginResponse.AuthorId)
                    .AddRoleClaim(loginResponse.Role)
                    .AddExpirationDateDays(180)
                    .Build();

                return Ok(new SuccessResponse<LoginResponse>(new LoginResponse($"Bearer {jwtToken}")));
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
            }
        }
    }
}
