using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.User.Commands.UpdateUser
{
    public record UpdateUserCommand(string FirstName, string LastName) : IRequest<Result<bool>>;
}
