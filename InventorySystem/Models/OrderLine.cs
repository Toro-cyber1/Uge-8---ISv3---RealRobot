namespace InventorySystem;

public class OrderLine
{
    public Item Item { get; set; } = null!;
    public int Quantity { get; set; }

    public decimal LineTotal => Item.PricePerUnit * Quantity;
}