using AutoMapper;
using Catalog.Application.Categories.Queries.GetCategories;
using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public CategoryDto CategoryToUpdate { get; set; }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.CategoryToUpdate);

            _context.Categories.Update(category);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
