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

        Logger.DisableDebug = false;
        Logger.DisableWarn = true;

        Logger.Info("Join2Start v1");
        Logger.Info("Developed by the Moonlight Panel Team and modified by Dannyx");

        Logger.Info("Waiting for players...");
        
        Listener = new TcpListener(IPAddress.Any, int.Parse(args[0]));
        
        Listener.Start();
        Listener.BeginAcceptTcpClient(OnClientConnected, null);

        await MonitorUserInput();
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
    
    private static async Task MonitorUserInput()
    {
        while (true)
        {
            string input = await Task.Run(Console.ReadLine); // Read user input

            if (input.Equals("stop", StringComparison.OrdinalIgnoreCase))
            {
                // Handle the "stop" condition here
                Logger.Info("Stopping the program and exiting...");
                Listener.Stop();
                break;
            }
        }
    }
}