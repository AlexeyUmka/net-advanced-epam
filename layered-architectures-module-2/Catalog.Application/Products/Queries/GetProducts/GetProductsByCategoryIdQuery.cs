using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Products.Queries.GetProducts;

public class GetProductsByCategoryIdQuery : IRequest<PaginatedList<ProductDto>>
{
    public int? CategoryId;
    public int PageNumber = 1;
    public int PageSize = 1;
    
    public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsByCategoryIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var products = _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == request.CategoryId)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            var paginatedProducts = await PaginatedList<ProductDto>.CreateAsync(products, request.PageNumber, request.PageSize);

            return paginatedProducts;
        }
    }
}
