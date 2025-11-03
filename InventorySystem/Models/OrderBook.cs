using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InventorySystem;

public class OrderBook
{
    public ObservableCollection<Order> Queued { get; } = new();
    public ObservableCollection<Order> Processed { get; } = new();

    public decimal TotalRevenue { get; private set; }
    
    public List<OrderLine> ProcessNextOrderAndReturnLines()
    {
        if (Queued.Count == 0)
            return new List<OrderLine>();

        var order = Queued[0];
        Queued.RemoveAt(0);
        Processed.Add(order);

        TotalRevenue += order.Total;
        return order.OrderLines;
    }
}