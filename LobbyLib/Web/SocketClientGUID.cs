using LobbyLib.CustomTicket;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using System.Text.Json;
using EIV_JsonLib.Lobby;
using EIV_JsonLib.Lobby.Group;

namespace LobbyLib.Web;

internal struct MapQueue
{
    public List<string> UserIds;
    public string Map;
}

internal struct Group
{
    public int Id;
    public string Owner;
    public List<string> UserIds;
    public List<string> InvitedUsers;
    public bool OwnerInvite;
}

internal partial class EIV_Lobby
{
    public static Dictionary<string, WebSocketStruct> ClientUserToWS = [];

    public static List<MapQueue> MapQueues = [];

    public static List<Group> Groups = [];

    static int LastSavedGroupId = 1;

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
            Control(data, socketStruct, ticketstruct.Value.UserId);
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

    public static void Control(ReadOnlySpan<byte> data, WebSocketStruct socketStruct, string userId)
    {
        try
        {
            ClientSocketMessage? message = JsonSerializer.Deserialize<ClientSocketMessage>(data);
            if (message == null)
                return;
            switch (message.Enum)
            {
                case ClientSocketEnum.None:
                    break;
                case ClientSocketEnum.MatchmakeCheck:
                    {
                        MatchmakeCheck? matchmakeCheck = JsonSerializer.Deserialize<MatchmakeCheck>(message.JsonMessage);
                        if (matchmakeCheck == null)
                            return;
                        var mapqueue = MapQueues.FirstOrDefault(x => x.Map == matchmakeCheck.Map);
                        if (string.IsNullOrEmpty(mapqueue.Map))
                            MapQueues.Add(new MapQueue()
                            {
                                Map = matchmakeCheck.Map,
                                UserIds =
                                [
                                    userId
                                ]
                            });
                        else if (!mapqueue.UserIds.Contains(userId))
                        {
                            mapqueue.UserIds.Add(userId);
                        }
                        //  We should send back something.
                        // And add logic for actually starting the game and the server respectively.
                    }
                    break;
                case ClientSocketEnum.GroupCreate:
                    {
                        if (Groups.Any(x => x.UserIds.Contains(userId)))
                        {
                            // send back we cant. (We already in a group.)
                            return;
                        }
                        Groups.Add(new Group()
                        {
                            Id = LastSavedGroupId,
                            InvitedUsers = [],
                            Owner = userId,
                            OwnerInvite = true,
                            UserIds =
                            [
                                userId
                            ]
                        });
                        LastSavedGroupId++;
                        // send back something? (GroupUpdate)
                    }
                    break;
                case ClientSocketEnum.GroupDelete:
                    {
                        if (!Groups.Any(x => x.Owner == userId))
                        {
                            // send back we cant. (We not an owner)
                            return;
                        }
                        Groups.Remove(Groups.First(x => x.Owner == userId));
                        // send back ok response?
                    }
                    break;
                case ClientSocketEnum.GroupUpdate:
                    {
                        GroupUpdate? groupUpdate = JsonSerializer.Deserialize<GroupUpdate>(message.JsonMessage);
                        if (groupUpdate == null)
                            return;
                        if (!Groups.Any(x => x.Id != groupUpdate.GroupId))
                        {
                            // send back we cant. (Hijack?)
                            return;
                        }
                        // TODO. Figure out this.
                    }
                    break;
                case ClientSocketEnum.GroupInvite:
                    {
                        GroupInvite? groupInvite = JsonSerializer.Deserialize<GroupInvite>(message.JsonMessage);
                        if (groupInvite == null)
                            return;
                        if (!ClientUserToWS.TryGetValue(groupInvite.Invitee, out var webSocketStruct))
                        {
                            // User not connected to websocket. Throw it out.
                            return;
                        }
                        webSocketStruct.SendWebSocketByteArray(data.ToArray());
                    }
                    break;
                case ClientSocketEnum.GroupInviteResponse:
                    {
                        // duh we missing who is responding and to what!!
                    }
                    break;
                case ClientSocketEnum.GroupKick:
                    {
                        GroupKick? groupKick = JsonSerializer.Deserialize<GroupKick>(message.JsonMessage);
                        if (groupKick == null)
                            return;
                        if (!ClientUserToWS.TryGetValue(groupKick.Invitee, out var webSocketStruct))
                        {
                            // User not connected to websocket. Throw it out.
                            return;
                        }
                        if (!Groups.Any(x => x.Owner == userId && x.Id == groupKick.GroupId))
                        {
                            // send back we cant. (We not an owner)
                            return;
                        }
                        webSocketStruct.SendWebSocketByteArray(data.ToArray());
                        Groups.First(x => x.Owner == userId).UserIds.Remove(groupKick.Invitee);
                    }
                    break;
                case ClientSocketEnum.FriendSearch:
                    break;
                case ClientSocketEnum.FriendSearchResponse:
                    break;
                case ClientSocketEnum.FriendAction:
                    break;
                case ClientSocketEnum.BlockListUpdate:
                    break;
                default:
                    break;
            }
        }
        catch
        {
            socketStruct.SendWebSocketClose(401, "Cannot Deserialize to ClientSocketMessage");
        }
    }
}
