using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.Id).NotEqual(0);
        
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(p => p.CategoryId).NotEqual(0);

        RuleFor(p => p.Price).NotEqual(0);
        
        RuleFor(p => p.Amount).NotEqual((uint)0);
    }
}
