using EIV_JsonLib.Interfaces;
using Newtonsoft.Json;

namespace LobbyLib.Jsons;

public class StashInventory
{
    /// <summary>
    /// ID from UserDB
    /// </summary>
    [JsonIgnore]
    public Guid UserId { get; set; }

    /// <summary>
    /// Only Compress and send Stash!
    /// </summary>
    public required IStash Stash { get; set; }
}
