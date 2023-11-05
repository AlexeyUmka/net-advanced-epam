using Carting.BLL.Models;
using FluentValidation;

namespace Carting.BLL.Validators;

public class CartItemValidator : AbstractValidator<CartItem>
{
    public CartItemValidator()
    {
        RuleFor(cartItem => cartItem.ExternalId).NotNull();
        RuleFor(cartItem => cartItem.Name).NotEmpty();
        RuleFor(cartItem => cartItem.Price).NotNull();
    }
}