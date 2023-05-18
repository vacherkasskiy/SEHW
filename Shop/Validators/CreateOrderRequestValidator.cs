using FluentValidation;
using Shop.Requests;

namespace Shop.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.Signature).NotNull();
        RuleFor(x => x.Signature).MaximumLength(255);
        RuleFor(x => x.Dishes).NotNull();
        RuleForEach(x => x.Dishes).SetValidator(new DishModelValidator());
    }
}