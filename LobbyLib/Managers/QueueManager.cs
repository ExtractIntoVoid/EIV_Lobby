using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;
using LobbyLib.Web;
using ModdableWebServer;
using System.Text.Json;

namespace LobbyLib.Managers;

public struct MapQueue
{
    public List<string> UserIds;
    public string Map;
}

public struct Map
{
    public string Name;
    public int MinPlayer;
    public int MaxPlayer;
}

public class QueueManager
{
    public static List<MapQueue> MapQueues { get; set; } = [];
    public static List<Map> Maps { get; set; } = [];

    public static void AddQueue(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage message)
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
                    ticket.UserId
                ]
            });
        else if (!mapqueue.UserIds.Contains(ticket.UserId))
        {
            mapqueue.UserIds.Add(ticket.UserId);
        }
        ClientSocketResponse response = new()
        {
            IsSuccess = true,
            ErrorCode = 0,
            Message = "You have been added to Queue!",
        };
        EIV_Lobby.SendResponse(socketStruct, response);
        CheckQueue();
    }

    public static void RemoveQueue(string userId)
    {
        var queue = MapQueues.FirstOrDefault(x=>x.UserIds.Contains(userId));
        // no queue.
        if (queue.UserIds.Count == 0)
            return;
        queue.UserIds.Remove(userId);
        if (queue.UserIds.Count == 0)
        {
            MapQueues.Remove(queue);
        }
    }

    // And add logic for actually starting the game and the server respectively.
    public static void CheckQueue()
    {
        foreach (var mapqueue in MapQueues)
        {
            var maps = Maps.FindAll(x=>x.Name == mapqueue.Map && x.MinPlayer <= mapqueue.UserIds.Count && x.MaxPlayer > mapqueue.UserIds.Count);
            foreach (var map in maps)
            {
                //  start the server.
                var result = GameStartManager.StartGameServer(map.Name);
                if (result.port == 0)
                    continue;
                foreach (var userId in mapqueue.UserIds)
                {
                    if (!EIV_Lobby.ClientUserToWS.TryGetValue(userId, out var webSocketStruct))
                    {
                        return;
                    }
                    EIV_Lobby.SendResponse(webSocketStruct, new GameStart()
                    { 
                        Address = result.ip,
                        Port = result.port
                    }, ClientSocketEnum.GameStart);
                }

            }
        }
    }

}
