using MediatR;
using Identity.Application.Dtos;
using SharedLibrary.Models;
using Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Utils;

namespace Identity.Application.Features.Authentication.Commands.UpdateAccessToken
{
    public class UpdateAccessTokenCommandHandler : IRequestHandler<UpdateAccessTokenCommand, Result<AuthenticationDto>>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UpdateAccessTokenCommandHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<AuthenticationDto>> Handle(UpdateAccessTokenCommand request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return Result<AuthenticationDto>.Failure("Bad authentication request");
            }

            var jwtHelper = new JwtHelper(_configuration, _httpContextAccessor);
            var IdentifierResult = jwtHelper.ExtractIdentifier();

            if (!IdentifierResult.IsSuccess)
            {
                return Result<AuthenticationDto>.Failure(IdentifierResult.Errors);
            }

            if (!await _userRepository.ExistsAsync(IdentifierResult.Value))
            {
                return Result<AuthenticationDto>.Failure("Identifier not recognized");
            }

            // Refresh token
            var refreshToken = await _refreshTokenRepository.GetValidTokenByUserAsync(IdentifierResult.Value, request.RefreshToken);

            if (refreshToken == null)
            {
                return Result<AuthenticationDto>.Failure("Authentication necessary to perform this action");
            }

            var refreshResult = jwtHelper.GenerateToken(IdentifierResult.Value, ETokenType.Refresh, refreshToken.ExpiresAt);

            if (!refreshResult.IsSuccess)
            {
                return Result<AuthenticationDto>.Failure(refreshResult.Errors);
            }

            refreshToken.Token = refreshResult.Value.Token;
            var updateRequest = _refreshTokenRepository.UpdateAsync(refreshToken);

            // Access token
            var accessResult = jwtHelper.GenerateToken(IdentifierResult.Value);

            if (!accessResult.IsSuccess)
            {
                return Result<AuthenticationDto>.Failure(accessResult.Errors);
            }

            await updateRequest;

            return Result<AuthenticationDto>.Success(new AuthenticationDto(accessResult.Value.Token, refreshResult.Value.Token));
        }
    }
}
