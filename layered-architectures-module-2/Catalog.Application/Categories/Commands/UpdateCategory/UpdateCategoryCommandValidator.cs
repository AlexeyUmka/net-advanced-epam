using Catalog.Application.Common.Interfaces;
using FluentValidation;

namespace Catalog.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(c => c.CategoryToUpdate.Name)
            .MaximumLength(50)
            .NotEmpty();
        
        RuleFor(c => !c.CategoryToUpdate.ParentCategoryId.HasValue || _context.Categories.Any(category => category.Id == c.CategoryToUpdate.ParentCategoryId)).Equal(true);
    }
}
