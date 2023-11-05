using Catalog.Application.Categories.Queries.GetCategories;
using Catalog.Application.Common.Interfaces;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Common.Models;
using Catalog.Application.Products.Commands.DeleteProduct;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Queries.GetProducts;

public class ProductDto : IMapFrom<Product>, IHaveHypermediaLinks
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? ImageUrl { get; set; }
    public int? CategoryId { get; set; }
    
    public CategoryDto? Category { get; set; }
    public decimal? Price { get; set; }
    public uint? Amount { get; set; }

    public IEnumerable<Link> HypermediaLinks
    {
        get =>
            new List<Link>()
            {
                new ("self", $"products/{Id}", HttpMethod.Get.ToString()),
                new ("self-delete", $"products/{Id}", HttpMethod.Delete.ToString(), new DeleteProductCommand() {Id = Id ?? -1}),
                new ("category", $"categories/{CategoryId}", HttpMethod.Get.ToString()),
            };
        set => throw new InvalidOperationException();
    }
}
