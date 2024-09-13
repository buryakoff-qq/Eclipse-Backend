using Eclipse.Core.DTOs.UserDTOs;
using FluentValidation;
using FluentValidation.Results;

namespace Eclipse.Core.Services.Validation
{
    public class RegisterValidation : AbstractValidator<UserRegistration>, IValidator<UserRegistration>
    {
        public RegisterValidation()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(2).WithMessage("Username must be at least 2 characters")
                .MaximumLength(30).WithMessage("Username must be at most 30 characters");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters");
            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters");
        }
    }
}
