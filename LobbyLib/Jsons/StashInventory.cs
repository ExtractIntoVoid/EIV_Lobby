using JsonLib.Interfaces;
using Newtonsoft.Json;

namespace LobbyLib.Jsons
{
    public class StashInventory
    {
        [JsonIgnore]
        public Guid UserId { get; set; } //ID From UserDB Id
        public required IStash Stash { get; set; }
    }
}
