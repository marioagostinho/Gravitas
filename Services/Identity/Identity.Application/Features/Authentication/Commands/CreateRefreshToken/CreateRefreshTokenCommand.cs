using MediatR;
using Identity.Application.Dtos;
using SharedLibrary.Models;

namespace Identity.Application.Features.Authentication.Commands.RefreshToken
{
    public record CreateRefreshTokenCommand(Guid Identifier) : IRequest<Result<AuthenticationDto>>;
}
