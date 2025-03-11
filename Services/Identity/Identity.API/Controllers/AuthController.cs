using Identity.Application.Features.Authentication.Commands.RefreshToken;
using Identity.Application.Features.Authentication.Query.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationQuery request)
        {
            var loginResult = await _mediator.Send(request);

            if (!loginResult.IsSuccess)
            {
                return BadRequest(loginResult.Errors);
            }

            var authResult = await _mediator.Send(new CreateRefreshTokenCommand(loginResult.Value));

            if (!authResult.IsSuccess)
            {
                return BadRequest(authResult.Errors);
            }

            return Ok(new { result = authResult.Value });
        }
    }
}
