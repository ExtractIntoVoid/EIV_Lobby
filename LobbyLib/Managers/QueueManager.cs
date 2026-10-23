using EIV_LobbyData.Socket.CustomMessages;
using LobbyLib.Web;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.Managers;

/// <summary>
/// Provides a manager for map queue.
/// </summary>
public static class QueueManager
{
    private static readonly List<Queue> StartedQueues = [];

    private static List<Queue> Queues { get; } = [];

    /// <summary>
    /// Add user to the queue.
    /// </summary>
    /// <param name="userId">Id of the user to add.</param>
    /// <param name="map">The map to check.</param>
    public static void AddQueue(string userId, string map)
    {
        var mapqueue = Queues.FirstOrDefault(x => x.Map == map);
        if (string.IsNullOrEmpty(mapqueue.Map))
        {
            Queues.Add(new Queue()
            {
                Map = map,
                UserIds =
                [
                    userId
                ],
            });
        }
        else if (!mapqueue.UserIds.Contains(userId))
        {
            mapqueue.UserIds.Add(userId);
        }

        CheckQueue();
    }

    /// <summary>
    /// Remove the user from the queue.
    /// </summary>
    /// <param name="userId">The user to remove.</param>
    public static void RemoveQueue(string userId)
    {
        var queue = Queues.FirstOrDefault(x => x.UserIds.Contains(userId));

        // no queue.
        if (queue.UserIds.Count == 0)
        {
            return;
        }

        queue.UserIds.Remove(userId);
        if (queue.UserIds.Count == 0)
        {
            Queues.Remove(queue);
        }
    }

    /// <summary>
    /// Checks the queue and starts the game.
    /// </summary>
    public static void CheckQueue()
    {
        if (Queues.Count == 0)
        {
            return;
        }

        if (GameDataStorage.Maps.Count == 0)
        {
            return;
        }

        StartedQueues.Clear();

        foreach (Queue queue in Queues)
        {
            var maps = GameDataStorage.Maps.Where(x => x.Name == queue.Map && x.MinPlayers <= queue.UserIds.Count && x.MaxPlayers > queue.UserIds.Count);
            foreach (var map in maps)
            {
                GameStartManager.StartGameServer(map.Name);

                StartedQueues.Add(queue);

                foreach (var userId in queue.UserIds)
                {
                    var clientUser = LobbyWeb.ClientUsers.FirstOrDefault(x => x.Key.UserId == userId);
                    if (clientUser.Key.UserId != userId)
                    {
                        continue;
                    }

                    // TODO: Send connection .
                }
            }
        }

        foreach (Queue queue in StartedQueues)
        {
            Queues.Remove(queue);
        }

        StartedQueues.Clear();
    }

    private struct Queue
    {
        public List<string> UserIds;
        public string Map;
    }
}
