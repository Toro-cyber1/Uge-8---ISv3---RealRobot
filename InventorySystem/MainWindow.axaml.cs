using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace InventorySystem;

public partial class MainWindow : Window
{
    private readonly OrderBook _orderBook = new();
    private readonly ItemSorterRobot _robot = new();
    
    public ObservableCollection<Order> Queued    => _orderBook.Queued;
    public ObservableCollection<Order> Processed => _orderBook.Processed;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _robot.Log = msg =>
            Dispatcher.UIThread.Post(() =>
                StatusMessages.Text += msg + Environment.NewLine);

        _robot.IpAddress = IpBox.Text ?? "127.0.0.1";
        _robot.DryRun = UseDryRun.IsChecked ?? true;

        var item1 = new UnitItem
        {
            Name = "M3 screw",
            PricePerUnit = 1m,
            InventoryLocation = 1
        };
        var item2 = new UnitItem
        {
            Name = "M3 nut",
            PricePerUnit = 1.5m,
            InventoryLocation = 2
        };
        var item3 = new UnitItem
        {
            Name = "pen",
            PricePerUnit = 1m,
            InventoryLocation = 3
        };

        var orderLine1 = new OrderLine { Item = item1, Quantity = 1 };
        var orderLine2 = new OrderLine { Item = item2, Quantity = 2 };
        var orderLine3 = new OrderLine { Item = item3, Quantity = 1 };

        var order1 = new Order
        {
            OrderLines = new() { orderLine1, orderLine2, orderLine3 },
            Time = DateTime.Now - TimeSpan.FromDays(2)
        };
        var order2 = new Order
        {
            OrderLines = new() { orderLine2 },
            Time = DateTime.Now
        };

        var customer1 = new Customer { Name = "Ramanda" };
        var customer2 = new Customer { Name = "Totoro" };

        customer1.CreateOrder(_orderBook, order1);
        customer2.CreateOrder(_orderBook, order2);

        UpdateTotals();
    }

    private async void OnProcessClick(object? sender, RoutedEventArgs e)
    {
        StatusMessages.Text += "Processing order…" + Environment.NewLine;

        var lines = _orderBook.ProcessNextOrderAndReturnLines();

        if (lines == null || lines.Count == 0)
        {
            StatusMessages.Text += "No queued orders." + Environment.NewLine;
            return;
        }

        foreach (var line in lines)
        {
            for (int i = 0; i < line.Quantity; i++)
            {
                StatusMessages.Text +=
                    $"Picking up {line.Item.Name} (slot {line.Item.InventoryLocation})" +
                    Environment.NewLine;

                _robot.PickUp(line.Item.InventoryLocation);
                
                await Task.Delay(9500); 
            }
        }

        UpdateTotals();
    }

    private void UpdateTotals()
    {
        TotalText.Text = $"Total revenue: {_orderBook.TotalRevenue:0.00} kr.";
    }

    private void OnApplyRobotSettingsClick(object? sender, RoutedEventArgs e)
    {
        _robot.IpAddress = IpBox.Text ?? "127.0.0.1";
        _robot.DryRun = UseDryRun.IsChecked ?? true;

        StatusMessages.Text +=
            $"Robot settings updated: IP={_robot.IpAddress}, DryRun={_robot.DryRun}{Environment.NewLine}";
    }
}
