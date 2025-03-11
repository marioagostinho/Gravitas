using MediatR;
using Identity.Domain.Repositories;
using SharedLibrary.Models;
using Identity.Application.Common;

namespace Identity.Application.Features.Authentication.Query.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(AuthenticationQuery request, CancellationToken cancellationToken)
        {
            var validator = new AuthenticationQueryValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (validatorResult.Errors.Any())
            {
                return Result<Guid>.Failure(validatorResult.Errors);
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return Result<Guid>.Failure("Identifier not recognized");
            }

            if (!EncryptionHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result<Guid>.Failure("Email or password invalid");
            }

            return Result<Guid>.Success(user.Id);
        }
    }
}