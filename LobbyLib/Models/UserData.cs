using EIV_LobbyData;
using System.Collections.ObjectModel;

namespace LobbyLib.Models;

/// <summary>
/// Represent a saved user data.
/// </summary>
public class UserData : CoreUserModel
{
    /// <summary>
    /// Gets or sets the user custom id.
    /// </summary>
    public string CustomId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the friend ids.
    /// </summary>
    public Collection<Guid> FriendsIds { get; init; } = [];

    /// <summary>
    /// Gets the friend request ids.
    /// </summary>
    public Collection<Guid> FriendRequests { get; init; } = [];

    /// <summary>
    /// Gets or sets the block list.
    /// </summary>
    public UserBlockList BlockList { get; set; } = new([], [], []);
}
