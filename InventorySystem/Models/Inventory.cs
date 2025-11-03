using System.Collections.Generic;
using System.Linq;

namespace InventorySystem;

public class Inventory
{
    // qty: stk for UnitItem, kg for BulkItem
    public Dictionary<Item, double> Stock { get; } = new();

    public void Add(Item item, double qty)
    {
        if (Stock.TryGetValue(item, out var cur)) Stock[item] = cur + qty;
        else Stock[item] = qty;
    }

    public bool TryConsume(Item item, double qty)
    {
        if (!Stock.TryGetValue(item, out var cur) || cur < qty) return false;
        Stock[item] = cur - qty;
        return true;
    }

    public List<Item> LowStockItems(double threshold) =>
        Stock.Where(kv => kv.Value < threshold).Select(kv => kv.Key).ToList();
}