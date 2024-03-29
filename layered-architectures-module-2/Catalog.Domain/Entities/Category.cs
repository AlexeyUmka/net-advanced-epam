﻿namespace Catalog.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int? ParentCategoryId { get; set; }
    
    public Category ParentCategory { get; set; }
    
    public IEnumerable<Category> ChildCategories { get; set; }
}
