using LobbyLib.CustomTicket;
using LobbyLib.Managers;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using System.Text.Json;
using EIV_JsonLib.Lobby;
using LobbyLib.Modding;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    public static Dictionary<string, WebSocketStruct> ClientUserToWS = [];

    [WS("/Socket/Client/{guid}")]
    public static void SocketClient(WebSocketStruct socketStruct)
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
            ControlClient(data, socketStruct, ticketstruct.Value);
        }
        if (socketStruct.IsConnected)
        {
            ClientUserToWS.Add(ticketstruct.Value.UserId, socketStruct);
        }
        if (socketStruct.IsClosed)
        {
            ClientUserToWS.Remove(ticketstruct.Value.UserId);
        }
    }

    public static void ControlClient(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, TicketStruct ticket)
    {
        try
        {
            ClientSocketMessage? message = JsonSerializer.Deserialize<ClientSocketMessage>(data);
            if (message == null)
                return;
            if (GroupManager.Manage(data, socketStruct, ticket, message.Enum, message))
                return;
            if (FriendManager.Manage(socketStruct, ticket, message.Enum, message))
                return;
            if (ItemActionManager.Manage(socketStruct, ticket, message.Enum, message))
                return;


            switch (message.Enum)
            {
                case ClientSocketEnum.MatchmakeCheck:
                    QueueManager.AddQueue(socketStruct, ticket, message);
                    break;
                default:
                    break;
            }

            foreach (var item in ModLoader.LobbyMods.Values)
            {
                item.ClientEvent(data, socketStruct, ticket, message.Enum, message);
            }
        }
        catch
        {
            socketStruct.SendWebSocketClose(401, "Cannot Deserialize to ClientSocketMessage");
        }
    }

    public static void SendResponse<T>(WebSocketStruct socketStruct, T obj, ClientSocketEnum @enum = ClientSocketEnum.SocketResponse)
    {
        ClientSocketMessage clientSocketMessage = new()
        {
            Enum = @enum,
            JsonMessage = JsonSerializer.Serialize(obj),
        };
        socketStruct.SendWebSocketText(JsonSerializer.Serialize(clientSocketMessage));
    }
}
