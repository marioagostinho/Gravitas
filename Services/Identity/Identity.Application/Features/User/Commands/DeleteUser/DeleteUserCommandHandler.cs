using AutoMapper;
using Identity.Application.Common;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Models;
using SharedLibrary.Utils;
using Entity = Identity.Domain.Entities;

namespace Identity.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IMapper mapper, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteUserCommandValidator();
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

            var user = await _userRepository.GetByIdAsync(IdentifierResult.Value);

            if (user == null)
            {
                return Result<bool>.Failure("Identifier not recognized");
            }

            if (!EncryptionHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result<bool>.Failure("Unauthorized");
            }

            return Result<bool>.Success(await _userRepository.DeleteAsync(user));
        }
    }
}
