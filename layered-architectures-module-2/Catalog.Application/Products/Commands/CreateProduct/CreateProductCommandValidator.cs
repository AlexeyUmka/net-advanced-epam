﻿using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.ProductToCreate.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(p => _context.Categories.Find(p.ProductToCreate.CategoryId)).NotNull();

        RuleFor(p => p.ProductToCreate.Price).GreaterThan(0);
        
        RuleFor(p => p.ProductToCreate.Amount).GreaterThan((uint)0);
    }
}