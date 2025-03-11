using FluentValidation;

namespace Identity.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("Fist name field cannot be empty.")
                .Length(5, 50).WithMessage("first name field must be between 5 and 50 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name field cannot be empty.")
                .Length(5, 50).WithMessage("Last name field must be between 5 and 50 characters.");
        }
    }
}
