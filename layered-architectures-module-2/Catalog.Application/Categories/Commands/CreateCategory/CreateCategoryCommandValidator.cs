using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(c => c.CategoryToCreate.Name)
            .MaximumLength(50)
            .NotEmpty();

        RuleFor(c => !c.CategoryToCreate.ParentCategoryId.HasValue || _context.Categories.Any(category => category.Id == c.CategoryToCreate.ParentCategoryId)).Equal(true);
    }
}
