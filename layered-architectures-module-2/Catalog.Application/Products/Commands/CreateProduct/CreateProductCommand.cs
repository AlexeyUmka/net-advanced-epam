using AutoMapper;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Products.Queries.GetProducts;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<int>
{
    public ProductDto ProductToCreate { get; set; }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request.ProductToCreate);

            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
