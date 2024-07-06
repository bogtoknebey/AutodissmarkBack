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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("auth-test-1")]
        public async Task<IActionResult> AuthTest1(CancellationToken ct)
        {
            var userId = HttpContext.GetUserId();
            var role = HttpContext.GetUserRole();

            return Ok(new SuccessResponse<string>($"Here is user with id:{userId} and role:{role}"));
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("auth-test-2")]
        public async Task<IActionResult> AuthTest2(CancellationToken ct)
        {
            return Ok(new SuccessResponse<string>("aaa"));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
        [HttpPost("auth-test-3")]
        public async Task<IActionResult> AuthTest3(CancellationToken ct)
        {
            return Ok(new SuccessResponse<string>("aaa"));
        }
    }
}
