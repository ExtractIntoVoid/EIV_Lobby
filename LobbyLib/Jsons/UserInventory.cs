using EIV_JsonLib;

namespace LobbyLib.Jsons;

public class UserInventory
{
    public Guid UserId { get; set; } //ID From UserDB Id
    public Inventory Inventory { get; set; } = new();
}
