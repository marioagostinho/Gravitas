using MediatR;
using AutoMapper;
using Identity.Application.Dtos;
using Identity.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Utils;

namespace Identity.Application.Features.User.Queries.GetUserDetails
{
    public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, Result<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public GetUserDetailsQueryHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IUserRepository userRepository)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Result<UserDto>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            // Validate token
            var jwtHelper = new JwtHelper(_configuration, _httpContextAccessor);
            var IdentifierResult = jwtHelper.ExtractIdentifier();

            if (!IdentifierResult.IsSuccess)
            {
                return Result<UserDto>.Failure(IdentifierResult.Errors);
            }

            if (!await _userRepository.ExistsAsync(IdentifierResult.Value))
            {
                return Result<UserDto>.Failure("Identifier not recognized");
            }

            if (IdentifierResult.Value != request.Id && !await _userRepository.ExistsAsync(IdentifierResult.Value))
            {
                return Result<UserDto>.Failure("User not found");
            }

            var user = await _userRepository.GetByIdAsync(request.Id ?? IdentifierResult.Value);
            var result = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(result);
        }
    }
}
