using EIV_JsonLib;
using Newtonsoft.Json;

namespace LobbyLib.Jsons;


// TODO: Talk with others how would we make the inventory.
public class UserInventory
{
    [JsonIgnore]
    public Guid UserId { get; set; } //ID From UserDB Id
    public Inventory Inventory { get; set; } = new();
}
