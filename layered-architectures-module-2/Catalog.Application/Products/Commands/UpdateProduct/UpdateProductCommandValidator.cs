using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.ProductToUpdate.Id).NotEqual(0);
        
        RuleFor(p => p.ProductToUpdate.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(p => _context.Categories.Find(p.ProductToUpdate.CategoryId)).NotNull();

        RuleFor(p => p.ProductToUpdate.Price).NotEqual(0);
        
        RuleFor(p => p.ProductToUpdate.Amount).NotEqual((uint)0);
    }
}
