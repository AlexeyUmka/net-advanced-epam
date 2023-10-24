using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IRequest<CategoriesVm>
{
    public int? Id;
    
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoriesVm> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                return new CategoriesVm()
                {
                    Categories = await _context.Categories.AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken)
                };
            }
        
            var category = await _context.Categories
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (category is null)
            {
                throw new NotFoundException();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
        
            return new CategoriesVm() { Categories = new [] { categoryDto } };
        }
    }
}
