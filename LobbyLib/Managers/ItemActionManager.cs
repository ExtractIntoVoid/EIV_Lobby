using EIV_JsonLib;
using EIV_JsonLib.Base;
using EIV_JsonLib.Extension;
using EIV_JsonLib.Json;
using EIV_JsonLib.Lobby;
using EIV_JsonLib.Lobby.ItemActions;
using LobbyLib.CustomTicket;
using LobbyLib.Web;
using ModdableWebServer;
using System.Text.Json;

namespace LobbyLib.Managers;

public class ItemActionManager
{
    public static bool Manage(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketEnum clientSocketEnum, ClientSocketMessage clientSocketMessage)
    {
        switch (clientSocketEnum)
        {
            case ClientSocketEnum.MoveItemAction:
                return false;
            case ClientSocketEnum.ConsumeItemAction:
                return false;
            case ClientSocketEnum.DiscardItemAction:
                DiscardItem(socketStruct, ticket, clientSocketMessage);
                return true;
            default:
                return false;
        }
    }

    public static void DiscardItem(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        var discard = JsonSerializer.Deserialize<DiscardAction>(clientSocketMessage.JsonMessage, ConvertHelper.GetSerializerSettings());
        if (discard == null)
            return;

        var stash = MainControl.Database.GetStashInventory(ticket.Id);
        if (stash == null)
            return;
        if (stash.Stash.Items.Remove(discard.ItemToDiscard))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveStashInventory(stash);
            return;
        }
        var inv = MainControl.Database.GetInventory(ticket.Id);
        if (inv == null)
            return;
        if (inv.Inventory.Items.Remove(discard.ItemToDiscard))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        // pain checks.
        if (inv.Inventory.Backpack == discard.ItemToDiscard)
        {
            inv.Inventory.Backpack = null;
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (inv.Inventory.Backpack != null && inv.Inventory.Backpack.Items.Remove(discard.ItemToDiscard))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (inv.Inventory.Hand == discard.ItemToDiscard)
        {
            inv.Inventory.Hand = null;
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (inv.Inventory.MeleeSlot == discard.ItemToDiscard)
        {
            inv.Inventory.MeleeSlot = null;
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (inv.Inventory.Primary == discard.ItemToDiscard)
        {
            inv.Inventory.Primary = null;
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (inv.Inventory.Secondary == discard.ItemToDiscard)
        {
            inv.Inventory.Secondary = null;
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }
        if (discard.ItemToDiscard is CoreArmor armor && armor != null && inv.Inventory.Armors.Remove(armor))
        {
            EIV_Lobby.SendResponse(socketStruct, new ClientSocketResponse()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Item succesfully discarded!",
            });
            MainControl.Database.SaveInventory(inv);
            return;
        }

    }

    public static void MoveItem(WebSocketStruct socketStruct, TicketStruct ticket, ClientSocketMessage clientSocketMessage)
    {
        var moveAction = JsonSerializer.Deserialize<MoveAction>(clientSocketMessage.JsonMessage, ConvertHelper.GetSerializerSettings());
        if (moveAction == null)
            return;
    }

    public static bool PlaceValidate(string place)
    {
        if (string.IsNullOrEmpty(place)) 
            return false;

        if (place == "stash" || place == "backpack")
            return true;

        return false;
    }
}
