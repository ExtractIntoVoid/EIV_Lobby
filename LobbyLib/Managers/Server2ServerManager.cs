using LobbyLib.INI;
using LobbyLib.WebClients;
using Serilog;
using System.Net;
using System.Text;

namespace LobbyLib.Managers;

/// <summary>
/// Manages server to server communication.
/// </summary>
public static class Server2ServerManager
{
    private static readonly List<IS2SClient> Clients = [];

    /// <summary>
    /// Initialize server to server communication.
    /// </summary>
    public static void Init()
    {
        if (!MainIni.Instance.Server2Server.Enabled)
        {
            return;
        }

        S2S_WssClient.OnReceivedEvent += S2S_OnReceivedEvent;
        S2S_WsClient.OnReceivedEvent += S2S_OnReceivedEvent;

        foreach (var client in MainIni.Instance.Server2Server.Connections)
        {
            WebSocketConnection(client);
        }
    }

    private static void S2S_OnReceivedEvent(IS2SClient client, byte[] data)
    {
        Log.Verbose("S2S Received communication on client: {data} {key}", Encoding.UTF8.GetString(data), client.Key);
    }

    private static void WebSocketConnection(ServerConnection connection)
    {
        if (connection.IsSecure)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            S2S_WssClient s2s_WssClient = new(connection.ConnectionKey, new(), new DnsEndPoint(connection.Address, connection.Port));
#pragma warning restore CA2000 // Dispose objects before losing scope

            s2s_WssClient.Connect();

            Clients.Add(s2s_WssClient);
        }
        else
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            S2S_WsClient s2s_WsClient = new(connection.ConnectionKey, new DnsEndPoint(connection.Address, connection.Port));
#pragma warning restore CA2000 // Dispose objects before losing scope

            s2s_WsClient.Connect();

            Clients.Add(s2s_WsClient);
        }
    }
}
