using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public int Id { get; set; }
    
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            await DeleteCategory(request.Id);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.Include(c => c.ChildCategories).FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                throw new NotFoundException(nameof(category), categoryId);
            }
            else if (category.ChildCategories?.Any() ?? false)
            {
                foreach (var childCategory in category.ChildCategories)
                {
                    await DeleteCategory(childCategory.Id);
                    _context.Categories.Remove(category);
                }
            }
            else
            {
                _context.Categories.Remove(category);
            }
        }
    }
}
