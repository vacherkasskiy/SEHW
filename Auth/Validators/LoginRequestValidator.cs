using Auth.Requests.Account;
using FluentValidation;

namespace Auth.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Email).MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.Password).MinimumLength(8);
    }
}