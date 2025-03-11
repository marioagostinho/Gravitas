using Identity.Application.Dtos;
using MediatR;
using SharedLibrary.Models;

namespace Identity.Application.Features.Authentication.Commands.UpdateAccessToken
{
    public record UpdateAccessTokenCommand(string RefreshToken) : IRequest<Result<AuthenticationDto>>;
}
