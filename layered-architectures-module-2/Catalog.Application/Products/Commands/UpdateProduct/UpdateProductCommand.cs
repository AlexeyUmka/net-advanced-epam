using AutoMapper;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Unit>, IMapFrom<Product>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public uint Amount { get; set; }
}

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
        var product = _mapper.Map<Product>(request);

        _context.Products.Update(product);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
