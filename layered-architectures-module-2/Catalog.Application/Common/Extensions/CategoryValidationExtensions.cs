using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Common.Extensions;

public static class CategoryValidationExtensions
{
    private static IApplicationDbContext _dbContext;
    public static void Configure(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public static IRuleBuilderOptions<T, TProperty> CategoryExists<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder.MustAsync(async (value, cancellationToken) =>
        {
            return (await _dbContext.Categories.FindAsync(value)) is not null;
        });
    }
}