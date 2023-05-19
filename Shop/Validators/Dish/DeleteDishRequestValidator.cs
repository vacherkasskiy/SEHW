using System.Data;
using FluentValidation;
using Shop.Requests.Dish;

namespace Shop.Validators;

public class DeleteDishRequestValidator : AbstractValidator<DeleteDishRequest>
{
    public DeleteDishRequestValidator()
    {
        RuleFor(x => x.Signature).NotNull();
        RuleFor(x => x.Signature).MaximumLength(255);
        RuleFor(x => x.DishId).NotNull();
        RuleFor(x => x.DishId).GreaterThan(0);
    }
}