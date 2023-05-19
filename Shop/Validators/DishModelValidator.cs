﻿using FluentValidation;
using Shop.Models;
using Shop.Requests;

namespace Shop.Validators;

public class DishModelValidator : AbstractValidator<DishModel>
{
    public DishModelValidator()
    {
        RuleFor(x => x.DishId).NotNull();
        RuleFor(x => x.DishId).GreaterThan(0);
        RuleFor(x => x.Quantity).NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}