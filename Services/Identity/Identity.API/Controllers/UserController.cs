using Identity.Application.Features.User.Commands.CreateUser;
using Identity.Application.Features.User.Commands.DeleteUser;
using Identity.Application.Features.User.Commands.UpdateUser;
using Identity.Application.Features.User.Queries.GetUserDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id?}")]
        [Authorize(AuthenticationSchemes = "AccessToken")]
        public async Task<IActionResult> Get(Guid? id)
        {
            var result = await _mediator.Send(new GetUserDetailsQuery(id));

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { result = result.Value });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { result = result.Value });
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = "AccessToken")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { result = result.Value });
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "AccessToken")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { result = result.Value });
        }
    }
}
