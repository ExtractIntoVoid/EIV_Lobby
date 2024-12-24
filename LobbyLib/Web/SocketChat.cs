using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using System.Text;
using System.Text.Json;
using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    public static Dictionary<TicketStruct, WebSocketStruct> ChatUserToWS = [];

    [WS("/EIV_Lobby/Socket/Chat")]
    public static void SocketChat(WebSocketStruct socketStruct)
    {
        if (!socketStruct.Request.Headers.TryGetValue("authorization", out var ticket))
        {
            socketStruct.SendWebSocketClose(401, "Authorization is not found!");
            return;
        }
        var ticketstruct = TicketProcess.GetTicket(ticket);
        if (ticketstruct == null)
        {
            socketStruct.SendWebSocketClose(401, "wrong ticket!");
            return;
        }

        Console.WriteLine("websocket hit!");

        if (socketStruct.WSRequest != null)
        {
            ReadOnlySpan<byte> data = socketStruct.WSRequest.Value.buffer.Skip((int)socketStruct.WSRequest.Value.offset).Take((int)socketStruct.WSRequest.Value.size).ToArray();
            ControlChat(data, socketStruct);
        }
        if (socketStruct.IsConnected)
        {
            ChatUserToWS.Add(ticketstruct.Value, socketStruct);
        }
        if (socketStruct.IsClosed)
        {
            ChatUserToWS.Remove(ticketstruct.Value);
        }
    }

    public static void ControlChat(ReadOnlySpan<byte> data, WebSocketStruct socketStruct)
    {

        try
        {
            var str = Encoding.UTF8.GetString(data);
            ChatMessage? chatMessage = JsonSerializer.Deserialize<ChatMessage>(str);
            if (chatMessage == null)
            {
                socketStruct.SendWebSocketClose(401, "No ChatMessage!");
                return;
            }
            // Better badword filter here.
            if (chatMessage.Message.Contains("badword"))
                return;
            var recUser = MainControl.Database.GetUserDatas().FirstOrDefault(x => x.UserId == chatMessage.ReceiverId);
            if (recUser == null)
                return;
            if (recUser.BlockList.FriendInviteBlocks.Contains(chatMessage.SenderId))
                return;

            var ticket = ChatUserToWS.Keys.FirstOrDefault(x => x.UserId == chatMessage.ReceiverId);
            if (string.IsNullOrEmpty(ticket.UserId))
                return;

            if (!ChatUserToWS.TryGetValue(ticket, out var webSocketStruct))
            {
                // User isnt active, should we store it to send to receiver? [Currently not.]
                return;
            }
            webSocketStruct.SendWebSocketByteArray(data.ToArray());
        }
        catch
        {
            socketStruct.SendWebSocketClose(401, "Wrong encoding!");
        }
    }
}
