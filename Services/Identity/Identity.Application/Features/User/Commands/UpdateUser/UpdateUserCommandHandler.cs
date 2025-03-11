using AutoMapper;
using Entity = Identity.Domain.Entities;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Utils;

namespace Identity.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IUserRepository userRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateUserCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (validatorResult.Errors.Any())
            {
                return Result<bool>.Failure(validatorResult.Errors);
            }

            var jwtHelper = new JwtHelper(_configuration, _httpContextAccessor);
            var IdentifierResult = jwtHelper.ExtractIdentifier();

            if (!IdentifierResult.IsSuccess)
            {
                return Result<bool>.Failure(IdentifierResult.Errors);
            }

            if (!await _userRepository.ExistsAsync(IdentifierResult.Value))
            {
                return Result<bool>.Failure("Identifier not recognized");
            }

            var user = _mapper.Map<Entity.User>(request);
            user.Id = IdentifierResult.Value;

            return Result<bool>.Success(await _userRepository.UpdateAsync(user));
        }
    }
}
