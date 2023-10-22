using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Common.Exceptions;
using Catalog.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<ProductsVm>
{
    public int? Id;
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ProductsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductsVm> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.Id is null)
        {
            return new ProductsVm()
            {
                Products = await _context.Products.AsNoTracking().ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken)
            };
        }
        
        var product = await _context.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException();
        }

        var productDto = _mapper.Map<ProductDto>(product);
        
        return new ProductsVm() { Products = new [] { productDto } };
    }
}
