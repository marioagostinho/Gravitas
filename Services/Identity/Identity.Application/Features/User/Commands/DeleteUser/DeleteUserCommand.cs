using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.User.Commands.DeleteUser
{
    public record DeleteUserCommand(string Password) : IRequest<Result<bool>>;
}
