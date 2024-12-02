using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using System.Text;
using System.Text.Json;
using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;

namespace LobbyLib.Web;

public class ChatWebsocket
{
    public static Dictionary<string, WebSocketStruct> UserToWS = [];

    [WS("/Socket/Chat")]
    public static void WSControl(WebSocketStruct socketStruct)
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
            Control(socketStruct.WSRequest.Value.buffer, socketStruct.WSRequest.Value.offset, socketStruct.WSRequest.Value.size, socketStruct);
        }
        if (socketStruct.IsConnected)
        {
            UserToWS.Add(ticketstruct.Value.UserId, socketStruct);
        }
        if (socketStruct.IsClosed)
        {
            UserToWS.Remove(ticketstruct.Value.UserId);
        }
    }

    public static void Control(byte[] buffer, long offset, long size, WebSocketStruct socketStruct)
    {
        if (size == 0)
            return;
        buffer = buffer.Take((int)size).ToArray();

        try
        {
            var str = Encoding.UTF8.GetString(buffer);
            ChatMessage? chatMessage = JsonSerializer.Deserialize<ChatMessage>(str);
            if (chatMessage == null)
            {
                socketStruct.SendWebSocketClose(401, "No ChatMessage!");
                return;
            }
            // Better badword filter here.
            if (chatMessage.Message.Contains("badword"))
                return;
            var recUser = MainControl.Database.GetUserData(chatMessage.ReceiverId);
            if (recUser == null)
                return;
            if (recUser.BlockList.FriendInviteBlocks.Contains(chatMessage.SenderId))
                return;
            if (!UserToWS.TryGetValue(chatMessage.ReceiverId, out var webSocketStruct))
            {
                // User isnt active, should we store it to send to receiver? [Currently not.]
                return;
            }
            webSocketStruct.SendWebSocketByteArray(buffer);
        }
        catch
        {
            socketStruct.SendWebSocketClose(401, "Wrong encoding!");
        }
    }
}
