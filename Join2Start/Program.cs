using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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

        var port = args.Length > 0 ? int.Parse(args[0]) : 25565;

        Logger.Info("Join2Start v1");
        Logger.Info("Developed by the Moonlight Panel Team");

        Logger.Info("Waiting for players");
        
        Logger.Info($"Using port {port}");
        
        Listener = new TcpListener(IPAddress.Any, port);
        
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