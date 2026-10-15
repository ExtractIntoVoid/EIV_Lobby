using EIV_Items;

namespace LobbyLib.Models;

/// <summary>
/// Represent a saved user stash.
/// </summary>
public class UserStash : CoreUserModel
{
    /// <summary>
    /// Gets or sets the stash.
    /// </summary>
    public required Stash Stash { get; set; }
}
