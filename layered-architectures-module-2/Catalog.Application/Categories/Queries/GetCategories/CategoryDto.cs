using Catalog.Application.Common.Mappings;
using Catalog.Domain.Entities;

namespace Catalog.Application.Categories.Queries.GetCategories;

public class CategoryDto : IMapFrom<Category>
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int? ParentCategoryId { get; set; }
    
    public CategoryDto ParentCategory { get; set; }
}
