using EIV_JsonLib.Extension;
using LobbyLib.CustomTicket;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{

    [HTTP("GET", "/EIV_Lobby/Profile/Inventory")]
    public static bool ProfileInventory(HttpRequest _, ServerStruct serverStruct)
    {
        if (!serverStruct.Headers.TryGetValue("authorization", out var ticket))
        {
            serverStruct.Response.MakeErrorResponse(401, "Authorization is not found!");
            serverStruct.SendResponse();
            return true;
        }
        var ticketstruct = TicketProcess.GetTicket(ticket);
        if (!ticketstruct.HasValue)
        {
            serverStruct.Response.MakeErrorResponse(401, "Wrong ticket!");
            serverStruct.SendResponse();
            return true;
        }

        var inventory = MainControl.Database.GetInventory(ticketstruct.Value.Id);
        if (inventory == null)
        {
            // create inventory
            inventory = new()
            { 
                UserId = ticketstruct.Value.Id,
                Inventory = new()
                {

                }
            };
            // TODO: Use EIV_Common Storage to fill this!.

            MainControl.Database.SaveInventory(inventory);
        }
        serverStruct.Response.MakeGetResponse(inventory.Inventory.Serialize());
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/EIV_Lobby/Profile/Stash")]
    public static bool ProfileStash(HttpRequest _, ServerStruct serverStruct)
    {
        if (!serverStruct.Headers.TryGetValue("authorization", out var ticket))
        {
            serverStruct.Response.MakeErrorResponse(401, "Authorization is not found!");
            serverStruct.SendResponse();
            return true;
        }
        var ticketstruct = TicketProcess.GetTicket(ticket);
        if (ticketstruct == null)
        {
            serverStruct.Response.MakeErrorResponse(401, "Wrong ticket!");
            serverStruct.SendResponse();
            return true;
        }
        var stash = MainControl.Database.GetStashInventory(ticketstruct.Value.Id);
        if (stash == null)
        {
            // create inventory
            stash = new()
            {
                UserId = ticketstruct.Value.Id,
                Stash = new()
                {

                }
            };
            // TODO: Use EIV_Common Storage to fill this!.

            MainControl.Database.SaveStashInventory(stash);
        }
        serverStruct.Response.MakeGetResponse(stash.Stash.Serialize());
        serverStruct.SendResponse();
        return true;
    }
}