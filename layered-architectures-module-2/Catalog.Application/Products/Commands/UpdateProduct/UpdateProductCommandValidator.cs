using Catalog.Application.Common.Extensions;
using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        RuleFor(p => p.ProductToUpdate.Id).NotEqual(0);
        
        RuleFor(p => p.ProductToUpdate.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(p => p.ProductToUpdate.CategoryId)
            .NotNull()
            .CategoryExists();

        RuleFor(p => p.ProductToUpdate.Price).NotEqual(0);
        
        RuleFor(p => p.ProductToUpdate.Amount).NotEqual((uint)0);
    }
}
