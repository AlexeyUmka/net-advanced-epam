﻿namespace Carting.DAL.Models;

public class CartItem
{
    public int ExternalId { get; set; }
    public string Name { get; set; }
    public Image Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}