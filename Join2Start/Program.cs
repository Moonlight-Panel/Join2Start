using System.Net;
using System.Net.Sockets;
using Logging.Net;
using YAMNL;

namespace Join2Start;

public static class Program
{
    private static TcpListener Listener = null!;
    
    public static async Task Main(string[] args)
    {
        // Disable YAMNL's logging

        Logger.DisableDebug = true;
        Logger.DisableWarn = true;

        Logger.Info("Join2Start v1");
        Logger.Info("Developed by the Moonlight Panel Team");

        Logger.Info("Waiting for players");
        
        Listener = new TcpListener(IPAddress.Any, int.Parse(args[0]));
        
        Listener.Start();
        Listener.BeginAcceptTcpClient(OnClientConnected, null);

        await Task.Delay(-1);
    }
    
    private static void OnClientConnected(IAsyncResult ar)
    {
        var client = Listener.EndAcceptTcpClient(ar);
        Listener.BeginAcceptTcpClient(OnClientConnected, null);
        
        _ = new MinecraftConnection(client, true)
        {
            PacketHandler = new Handler()
        };
    }
}