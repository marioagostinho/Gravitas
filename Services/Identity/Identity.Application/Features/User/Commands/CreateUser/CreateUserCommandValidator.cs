using FluentValidation;

namespace Identity.Application.Features.User.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email field cannot be empty.")
                .EmailAddress().WithMessage("Email field must be have an email format.")
                .Length(5, 50).WithMessage("Email field must be between 5 and 50 characters.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password field cannot be empty.")
                .Length(5, 20).WithMessage("Password field must be between 5 and 20 characters.");

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("Fist name field cannot be empty.")
                .Length(5, 50).WithMessage("first name field must be between 5 and 50 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name field cannot be empty.")
                .Length(5, 50).WithMessage("Last name field must be between 5 and 50 characters.");
        }
    }
}
