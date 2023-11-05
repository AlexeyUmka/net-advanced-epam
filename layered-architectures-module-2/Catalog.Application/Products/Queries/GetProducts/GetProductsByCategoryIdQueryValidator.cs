﻿using Catalog.Application.Common.Extensions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Products.Queries.GetProducts;
using FluentValidation;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class GetProductsByCategoryIdQueryValidator : AbstractValidator<GetProductsByCategoryIdQuery>
{
    private readonly IApplicationDbContext _context;

    public GetProductsByCategoryIdQueryValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(q => q.PageSize).LessThanOrEqualTo(100);
        
        RuleFor(q => q.CategoryId)
            .NotNull()
            .CategoryExists();
        
        RuleFor(q => q.PageNumber).GreaterThan(0);
    }
}
