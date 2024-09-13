using Eclipse.Core.DTOs.UserDTOs;
using FluentValidation;

namespace Eclipse.Core.Services.Validation
{
    public class LoginValidation : AbstractValidator<UserLogin>, IValidator<UserLogin>
    {
        public LoginValidation()
        {
            RuleFor(u => u.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(2).WithMessage("Username must be at least 2 characters")
                .MaximumLength(30).WithMessage("Username must be at most 30 characters");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }
}
