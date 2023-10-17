namespace Carting.DAL.Models;

public class Cart : EntityBase
{
    public int? ExternalId { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
}