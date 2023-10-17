namespace Carting.BLL.Models;

public class Cart
{
    public Cart()
    {
        Items = new List<CartItem>();
    }
    public string Id { get; set; }
    public int? ExternalId { get; set; }
    public IEnumerable<CartItem> Items { get; set; }
}