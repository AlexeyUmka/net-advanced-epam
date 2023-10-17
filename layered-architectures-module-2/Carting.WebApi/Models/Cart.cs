namespace Carting.WebApi.Models;

public class Cart
{
    public int ExternalId { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
}