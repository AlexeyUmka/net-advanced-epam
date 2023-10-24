using AutoMapper;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Products.Queries.GetProducts;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Unit>, IMapFrom<Product>
{
    public ProductDto ProductToUpdate { get; set; }
    
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request.ProductToUpdate);

            _context.Products.Update(product);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
