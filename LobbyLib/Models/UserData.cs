using EIV_JsonLib.Lobby;

namespace LobbyLib.Models;

public class UserData
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> FriendsIds { get; set; } = [];
    public List<string> FriendRequests { get; set; } = [];
    public UserBlockList BlockList { get; set; } = new();
}
