using System;
using System.Net.Sockets;
using System.Text;

namespace InventorySystem;

public class Robot
{
    public const int urscriptPort = 30002, dashboardPort = 29999;
    
    public string IpAddress = "127.0.0.1";
    
    public bool DryRun { get; set; } = true;
    
    public Action<string>? Log { get; set; }

    public void SendString(int port, string message)
    {
        if (DryRun)
        {
            Log?.Invoke($"[DryRun] -> {IpAddress}:{port}\n{message.Trim()}\n");
            return;
        }

        try
        {
            using var client = new TcpClient();
            client.Connect(IpAddress, port);
            using var stream = client.GetStream();
            var bytes = Encoding.ASCII.GetBytes(message);
            stream.Write(bytes, 0, bytes.Length);
            Log?.Invoke($"[Sent] {bytes.Length} bytes to {IpAddress}:{port}");
        }
        catch (Exception ex)
        {
            Log?.Invoke($"[Robot offline] {IpAddress}:{port} — {ex.GetType().Name}: {ex.Message}");
        }
    }

    public void SendUrscript(string urscript)
    {
        SendString(dashboardPort, "brake release\n");
        SendString(urscriptPort, urscript);
    }
}