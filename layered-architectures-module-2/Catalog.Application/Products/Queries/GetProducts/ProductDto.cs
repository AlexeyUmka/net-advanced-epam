using Catalog.Application.Categories.Queries.GetCategories;
using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Queries.GetProducts;

public class ProductDto : IMapFrom<Product>
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int CategoryId { get; set; }
    
    public CategoryDto Category { get; set; }
    
    public decimal Price { get; set; }

    public uint Amount { get; set; }
}
