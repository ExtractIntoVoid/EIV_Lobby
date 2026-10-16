using EIV_LobbyData;
using EIV_LobbyData.Socket;
using LobbyLib.CustomMessageProcessor;
using LobbyLib.Managers;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Interfaces;
using ModdableWebServer.Senders;
using Serilog;
using System.Text.Json;

namespace LobbyLib.Web;

/// <summary>
/// Represent a Lobby Web communication.
/// </summary>
internal partial class LobbyWeb
{
    /// <summary>
    /// Gets a ticket to session dictionary.
    /// </summary>
    public static Dictionary<TicketModel, IWSSession> ClientUsers { get; } = [];

    /// <summary>
    /// <see cref="SocketClient(WebSocketSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
#pragma warning disable CA2243 // Attribute string literals should parse correctly
    [WS("/Socket/Client/{guid}", WebSocketMethodListen.All)]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    public static void SocketClient(WebSocketSender sender)
    {
        IWSSession session = (IWSSession)sender.Session;

        if (!sender.Headers.TryGetValue("authorization", out var ticket))
        {
            session.SendClose(401, "Authorization is not found!");
            return;
        }

        var ticketstruct = TicketManager.GetTicket(ticket);
        if (ticketstruct == null)
        {
            session.SendClose(401, "wrong ticket!");
            return;
        }

        Log.Information("websocket client hit!");

        switch (sender.CurrentMethod)
        {
            case WebSocketMethodListen.Connected:
                ClientUsers.Add(ticketstruct, session);
                return;
            case WebSocketMethodListen.Disconnected:
                ClientUsers.Remove(ticketstruct);
                return;
            case WebSocketMethodListen.Received:
                {
                    if (!JsonSerializerConvert.TryGetTypeInfo<ClientMessage>(out var typeInfo))
                    {
                        return;
                    }

#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        ClientMessage? message = JsonSerializer.Deserialize(sender.Buffer, typeInfo);
                        if (message == null)
                        {
                            return;
                        }

                        ProcessorStorage.Process(ticketstruct, session, message.Message);
                    }
                    catch (Exception)
                    {
                        session.SendClose(401, "Cannot Deserialize Message!");
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                }

                return;
            case WebSocketMethodListen.Close:
                return;
            default:
                return;
        }
    }
}
