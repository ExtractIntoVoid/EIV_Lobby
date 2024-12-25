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
        var inv = MainControl.Database.GetProfile(ticket.Id);
        if (inv == null)
            return;
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

        return 
            place == "stash" ||
            place == "backpack" ||
            place == "hand" ||
            place == "toolbelt" ||
            place == "wearable_item";
    }
}
