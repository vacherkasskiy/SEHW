using FluentValidation;
using Shop.Requests;

namespace Shop.Validators;

public class GetInfoRequestValidator : AbstractValidator<GetInfoRequest>
{
    public GetInfoRequestValidator()
    {
        RuleFor(x => x.OrderId).NotNull();
        RuleFor(x => x.OrderId).GreaterThan(0);
    }
}