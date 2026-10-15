using EIV_LobbyData;
using EIV_LobbyData.Socket;
using LobbyLib.Managers;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Interfaces;
using ModdableWebServer.Senders;
using Serilog;
using System.Text;
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
    public static Dictionary<TicketModel, IWSSession> ChatUserToWS { get; } = [];

    /// <summary>
    /// <see cref="SocketChat(WebSocketSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    [WS("/Socket/Chat", WebSocketMethodListen.All)]
    public static void SocketChat(WebSocketSender sender)
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

        Log.Information("websocket hit!");

        switch (sender.CurrentMethod)
        {
            case WebSocketMethodListen.Connected:
                ChatUserToWS.Add(ticketstruct, session);
                break;
            case WebSocketMethodListen.Disconnected:
                ChatUserToWS.Remove(ticketstruct);
                break;
            case WebSocketMethodListen.Received:
                ControlChat(sender.Buffer, session);
                break;
            case WebSocketMethodListen.Close:
                break;
            default:
                break;
        }
    }

    private static void ControlChat(ReadOnlySpan<byte> data, IWSSession session)
    {
        if (!JsonSerializerConvert.TryGetTypeInfo<ChatMessage>(out var typeInfo))
        {
            return;
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            var str = Encoding.UTF8.GetString(data);
            ChatMessage? chatMessage = JsonSerializer.Deserialize(str, typeInfo);
            if (chatMessage == null)
            {
                session.SendClose(401, "No ChatMessage!");
                return;
            }

            // Better badword filter here.
            if (chatMessage.Message.Contains("badword", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var recUser = MainControl.Database.GetUserDatas().FirstOrDefault(x => x.Id == chatMessage.Receiver);
            if (recUser == null)
            {
                return;
            }

            if (recUser.BlockList.FriendInviteBlocks.Contains(chatMessage.Sender))
            {
                return;
            }

            var ticket = ChatUserToWS.Keys.FirstOrDefault(x => x.Id == chatMessage.Sender);
            if (ticket == null || string.IsNullOrEmpty(ticket.UserId))
            {
                return;
            }

            if (!ChatUserToWS.TryGetValue(ticket, out var webSocketStruct))
            {
                // User isnt active, should we store it to send to receiver? [Currently not.]
                return;
            }

            webSocketStruct.Send(data.ToArray());
        }
        catch (Exception)
        {
            session.SendClose(401, "Wrong encoding!");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
