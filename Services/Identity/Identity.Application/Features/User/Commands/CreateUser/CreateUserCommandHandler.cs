using MediatR;
using Identity.Application.Common;
using Identity.Domain.Repositories;
using Entity = Identity.Domain.Entities;
using SharedLibrary.Models;

namespace Identity.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        { 
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator();
            var validatorResult = await validator.ValidateAsync(request);

            if (validatorResult.Errors.Any())
            {
                return Result<string>.Failure(validatorResult.Errors);
            }

            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                return Result<string>.Failure("Email already exists.");
            }

            var (hash, salt) = EncryptionHelper.HashPassword(request.Password);
            var user = new Entity.User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email, 
                PasswordHash = hash,
                PasswordSalt = salt
            };

            var result = await _userRepository.CreateAsync(user);
            return Result<string>.Success(result.ToString());
        }
    }
}
