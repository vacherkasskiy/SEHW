using FluentValidation;
using Shop.Requests.Dish;

namespace Shop.Validators;

public class EditDishRequestValidator : AbstractValidator<EditDishRequest>
{
    public EditDishRequestValidator()
    {
        RuleFor(x => x.Signature).NotNull();
        RuleFor(x => x.Signature).MaximumLength(255);
        RuleFor(x => x.DishId).NotNull();
        RuleFor(x => x.DishId).GreaterThan(0);
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).MaximumLength(100);
        RuleFor(x => x.Price).NotNull();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Quantity).NotNull();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}