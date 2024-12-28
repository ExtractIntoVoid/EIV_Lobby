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
    
    [HTTP("POST", "/Connect")]
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

            UserProfile profile = new()
            {
                UserId = user.Id,
                Character = new()
                {
                    Name = user.Name,
                    CreationDate = DateTime.Now,
                    Inventory = new(),
                    Modules = new(),
                    Origin = string.Empty,
                }
            };
            string origin = ConfigINI.Read("Config.ini", "Default", "DefaultOrigin");
            //  Additional Datas must be "Key1=Value1;Key2=Value2".
            if (userinfo.AdditionalData.Contains("origin"))
            {
                origin = userinfo.AdditionalData.Split("origin=")[1].Split(";")[0];
            }

            if (Storage.Inventories.TryGetValue(ConfigINI.Read("Config.ini", "Default", "DefaultInventoryName"), out var out_inventory))
                profile.Character.Inventory = out_inventory;
            if (Storage.OriginToModules.TryGetValue(origin, out var out_modules))
                profile.Character.Modules = out_modules;
            MainControl.Database.SaveProfile(profile);

        }
        serverStruct.Response.MakeGetResponse(JsonSerializer.Serialize(new ConnectResponse()
        {
            Id = user.Id,
            Ticket = TicketProcess.CreateTicket(user),
        }));
        serverStruct.SendResponse();
        return true;
    }
}
