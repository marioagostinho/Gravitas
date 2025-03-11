using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.User.Commands.CreateUser
{
    public record CreateUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest<Result<string>>;
}
