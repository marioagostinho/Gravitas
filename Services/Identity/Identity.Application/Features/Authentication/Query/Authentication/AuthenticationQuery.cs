using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.Authentication.Query.Authentication
{
    public record AuthenticationQuery(string Email, string Password) : IRequest<Result<Guid>>;
}
