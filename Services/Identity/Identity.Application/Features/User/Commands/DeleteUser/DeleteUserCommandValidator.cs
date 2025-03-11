using FluentValidation;
using Identity.Domain.Repositories;

namespace Identity.Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .Length(5, 20).WithMessage("Password field must be between 5 and 20 characters.");
        }
    }
}
