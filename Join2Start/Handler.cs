using YAMNL;
using YAMNL.Data;
using YAMNL.Handshaking.Serverbound;
using YAMNL.Login.Clientbound;
using YAMNL.Login.Serverbound;
using YAMNL.Status.Clientbound;
using YAMNL.Status.Serverbound;
using PacketPing = YAMNL.Status.Serverbound.PacketPing;

namespace Join2Start;

public class Handler : IPacketHandler
{
    public async Task HandleIncomming(IPacketPayload packetPayload, MinecraftConnection connection)
    {
        if (packetPayload is PacketSetProtocol setProtocol)
        {
            if (setProtocol.NextState == 1)
            {
                connection.ConnectionState = ConnectionState.Status;
            }

            if (setProtocol.NextState == 2)
            {
                connection.ConnectionState = ConnectionState.Login;
            }
        }

        if (packetPayload is PacketPingStart)
        {
            await connection.SendPacket(new PacketServerInfo(
                "{\"version\":{\"name\":\"\u00A7bMoonlight Join2Start\u00A7f v.1\u00A7r\",\"protocol\":1},\"players\":{\"max\":1,\"online\":0,\"sample\":null},\"description\":{\"text\":\"\u00A7eConnect to this server to \u00A7astart\u00A7r\\n\u00A7fDeveloped by the \u00A7 Moonlight Panel Team\u00A7r\"},\"favicon\":\"data:image/png;base64,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\"}"));
        }

        if (packetPayload is PacketPing ping)
        {
            await connection.SendPacket(new YAMNL.Status.Clientbound.PacketPing(ping.Time));
        }

        if (packetPayload is PacketLoginStart start)
        {
            var s =
                "[\"\",{\"text\":\"===========================\n \u0020 \u0020 Hey " + start.Username +
                ", the server will \"},{\"text\":\"start now\",\"color\":\"green\"},{\"text\":\"\n \u0020Please reconnect in \"},{\"text\":\"1-3\",\"color\":\"aqua\"},{\"text\":\"\n \u0020minutes again\n===========================\"}]";

            await connection.SendPacket(new PacketDisconnect(s));

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Player connected. Exiting");
                Environment.Exit(0);
            });
        }
    }

    public Task HandleOutgoing(IPacketPayload packetPayload, MinecraftConnection connection)
    {
        return Task.CompletedTask;
    }
}