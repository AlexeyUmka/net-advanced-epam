using Catalog.Domain.Interfaces;

namespace Catalog.Domain.Entities;

public class Product : INotifyWhenUpdated
{
    public event EventHandler? Updated = ProductDomainEventsHandlers.ProductUpdatedHandler;
    public void IAmUpdated()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }

    public int Id { get; set; }

    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string ImageUrl { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
    
    public decimal Price { get; set; }

    public uint Amount { get; set; }
}
