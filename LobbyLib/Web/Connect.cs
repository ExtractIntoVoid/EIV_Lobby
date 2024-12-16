using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using NetCoreServer;
using System.Text.Json;
using EIV_Common.InfoJson;
using EIV_Common;
using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;
using EIV_Common.JsonStuff;
using LobbyLib.Jsons;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    
    [HTTP("POST", "/EIV_Lobby/Connect")]
    public static bool LobbyConnect(HttpRequest request, ServerStruct serverStruct)
    {
        if (string.IsNullOrEmpty(request.Body))
        {
            serverStruct.Response.MakeErrorResponse();
            serverStruct.SendResponse();
            return true;
        }

        var userinfo = JsonSerializer.Deserialize<UserInfoJson>(request.Body);
        if (userinfo == null)
        {
            serverStruct.Response.MakeErrorResponse("UserInfoJson cannot parse");
            serverStruct.SendResponse();
            return true;
        }

        var range = SemanticVersioning.Range.Parse(ConfigINI.Read("Lobby.ini", "Lobby", "Version"));
        var version = SemanticVersioning.Version.Parse(userinfo.Version);

        if (!range.IsSatisfied(version, true))
        {
            serverStruct.Response.MakeErrorResponse("VersionCheck");
            serverStruct.SendResponse();
            return true;
        }

        var user = MainControl.Database.GetUserDatas().FirstOrDefault(x => x.UserId == userinfo.CreateUserId());
        if (user == null)
        {
            user = new()
            {
                Id = Guid.NewGuid(),
                UserId = userinfo.CreateUserId(),
                Name = userinfo.Name,
                FriendsIds = [],
                BlockList = new(),
                FriendRequests = [],
            };
            MainControl.Database.SaveUserData(user);
            // create stash
            StashInventory stash = new()
            {
                UserId = user.Id,
                Stash = new()
            };
            if (Storage.Stashes.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultStashName"), out var out_stash))
                stash.Stash = out_stash;
            MainControl.Database.SaveStashInventory(stash);

            UserInventory inventory = new()
            {
                UserId = user.Id,
                Inventory = new()
            };
            if (Storage.Inventories.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultInventoryName"), out var out_inventory))
                inventory.Inventory = out_inventory;
            MainControl.Database.SaveInventory(inventory);

        }
        var ticket = TicketProcess.CreateTicket(user);

        ConnectResponse connectResponse = new()
        { 
            Id = user.Id,
            Ticket = ticket,
        };

        serverStruct.Response.MakeGetResponse(JsonSerializer.Serialize(connectResponse));
        serverStruct.SendResponse();
        return true;
    }
}
