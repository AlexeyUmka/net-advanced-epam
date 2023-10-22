using AutoMapper;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<int>, IMapFrom<Category>
{
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int? ParentCategoryId { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Category>(request);

        _context.Categories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
