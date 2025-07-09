using EIV_Common;
using EIV_Common.JsonStuff;
using EIV_JsonLib.Extension;
using LobbyLib.CustomTicket;
using ModdableWebServer;
using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{

    [HTTP("GET", "/Profile/Character")]
    public static bool ProfileCharacter(HttpRequest _, ServerStruct serverStruct)
    {
        if (!serverStruct.Headers.TryGetValue("authorization", out var ticket))
        {
            serverStruct.Response.MakeErrorResponse(401, "Authorization is not found!");
            serverStruct.SendResponse();
            return true;
        }
        var ticketstruct = TicketManager.GetTicket(ticket);
        if (!ticketstruct.HasValue)
        {
            serverStruct.Response.MakeErrorResponse(401, "Wrong ticket!");
            serverStruct.SendResponse();
            return true;
        }

        var profile = MainControl.Database.GetProfile(ticketstruct.Value.Id);
        if (profile == null)
        {
            profile = new()
            { 
                UserId = ticketstruct.Value.Id,
                Character = new()
                {
                    Name = ticketstruct.Value.Name,
                    CreationDate = DateTime.Now,
                    Inventory = new(),
                    Modules = new(),
                    Origin = string.Empty
                }
            };
            if (Storage.Inventories.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultInventoryName"), out var out_inventory))
                profile.Character.Inventory = out_inventory;
            if (Storage.OriginToModules.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultOrigin"), out var out_modules))
                profile.Character.Modules = out_modules;
            MainControl.Database.SaveProfile(profile);
        }
        serverStruct.Response.MakeGetResponse(profile.Character.Serialize());
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/Profile/Inventory")]
    public static bool ProfileInventory(HttpRequest _, ServerStruct serverStruct)
    {
        if (!serverStruct.Headers.TryGetValue("authorization", out var ticket))
        {
            serverStruct.Response.MakeErrorResponse(401, "Authorization is not found!");
            serverStruct.SendResponse();
            return true;
        }
        var ticketstruct = TicketManager.GetTicket(ticket);
        if (!ticketstruct.HasValue)
        {
            serverStruct.Response.MakeErrorResponse(401, "Wrong ticket!");
            serverStruct.SendResponse();
            return true;
        }

        var profile = MainControl.Database.GetProfile(ticketstruct.Value.Id);
        if (profile == null)
        {
            profile = new()
            {
                UserId = ticketstruct.Value.Id,
                Character = new()
                {
                    Name = ticketstruct.Value.Name,
                    CreationDate = DateTime.Now,
                    Inventory = new(),
                    Modules = new(),
                    Origin = string.Empty
                }
            };
            if (Storage.Inventories.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultInventoryName"), out var out_inventory))
                profile.Character.Inventory = out_inventory;
            if (Storage.OriginToModules.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultOrigin"), out var out_modules))
                profile.Character.Modules = out_modules;
            MainControl.Database.SaveProfile(profile);
        }
        serverStruct.Response.MakeGetResponse(profile.Character.Inventory.Serialize());
        serverStruct.SendResponse();
        return true;
    }

    [HTTP("GET", "/Profile/Stash")]
    public static bool ProfileStash(HttpRequest _, ServerStruct serverStruct)
    {
        if (!serverStruct.Headers.TryGetValue("authorization", out var ticket))
        {
            serverStruct.Response.MakeErrorResponse(401, "Authorization is not found!");
            serverStruct.SendResponse();
            return true;
        }
        var ticketstruct = TicketManager.GetTicket(ticket);
        if (ticketstruct == null)
        {
            serverStruct.Response.MakeErrorResponse(401, "Wrong ticket!");
            serverStruct.SendResponse();
            return true;
        }
        var stash = MainControl.Database.GetStashInventory(ticketstruct.Value.Id);
        if (stash == null)
        {
            // create stash
            stash = new()
            {
                UserId = ticketstruct.Value.Id,
                Stash = new()
            };
            if (Storage.Stashes.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultStashName"), out var out_stash))
                stash.Stash = out_stash;
            MainControl.Database.SaveStashInventory(stash);
        }
        serverStruct.Response.MakeGetResponse(stash.Stash.Serialize());
        serverStruct.SendResponse();
        return true;
    }
}