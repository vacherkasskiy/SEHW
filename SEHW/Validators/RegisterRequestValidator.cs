using Data;
using FluentValidation;
using SEHW.Requests.Account;

namespace SEHW.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Username).MaximumLength(50);
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Email).MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.Role).MaximumLength(10);
    }
}