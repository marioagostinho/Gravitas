using MediatR;
using Identity.Application.Dtos;
using Entity = Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Utils;

namespace Identity.Application.Features.Authentication.Commands.RefreshToken
{
    public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand, Result<AuthenticationDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public CreateRefreshTokenCommandHandler(IConfiguration configuration, IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<AuthenticationDto>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Identifier == Guid.Empty)
            {
                return Result<AuthenticationDto>.Failure("Bad authentication request");
            }

            if (!await _userRepository.ExistsAsync(request.Identifier))
            {
                return Result<AuthenticationDto>.Failure("Identifier not recognized");
            }

            var jwtHelper = new JwtHelper(_configuration);

            // Refresh token
            var refreshResult = jwtHelper.GenerateToken(request.Identifier, ETokenType.Refresh);

            if (!refreshResult.IsSuccess)
            {
                return Result<AuthenticationDto>.Failure(refreshResult.Errors);
            }

            var refreshToken = new Entity.RefreshToken(request.Identifier, refreshResult.Value.Token, refreshResult.Value.ExpiresAt);
            var registerRequest = _refreshTokenRepository.CreateAsync(refreshToken);

            // Access token
            var accessResult = jwtHelper.GenerateToken(request.Identifier);

            if (!accessResult.IsSuccess)
            {
                return Result<AuthenticationDto>.Failure(accessResult.Errors);
            }

            await registerRequest;

            return Result<AuthenticationDto>.Success(new AuthenticationDto(accessResult.Value.Token, refreshResult.Value.Token));
        }
    }
}
