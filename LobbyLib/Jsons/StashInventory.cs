using EIV_JsonLib;

namespace LobbyLib.Jsons;

public class StashInventory
{
    /// <summary>
    /// ID from UserDB
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Only Compress and send Stash!
    /// </summary>
    public required Stash Stash { get; set; }
}
