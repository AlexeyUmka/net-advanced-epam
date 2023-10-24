namespace Catalog.Application.Products.Queries.GetProducts;

public class ProductsVm
{
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
}