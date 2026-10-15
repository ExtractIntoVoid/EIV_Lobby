using EIV_LobbyData;
using LobbyLib.INI;
using LobbyLib.Managers;
using LobbyLib.Models;
using ModdableWebServer.Attributes;
using ModdableWebServer.Senders;
using System.Text.Json;

namespace LobbyLib.Web;

/// <summary>
/// Represent a Lobby Web communication.
/// </summary>
internal partial class LobbyWeb
{
    /// <summary>
    /// <see cref="Connect(ServerSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <returns>ToSkip.</returns>
    [HTTP("POST", "/Connect")]
    public static bool Connect(ServerSender sender)
    {
        if (string.IsNullOrEmpty(sender.Request.Body))
        {
            sender.Response.MakeErrorResponse();
            sender.SendResponse();
            return true;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<ConnectResponse>(out var connectResponseTypeInfo))
        {
            sender.Response.MakeErrorResponse("Cannot get ConnectResponse typeInfo");
            sender.SendResponse();
            return true;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<ConnectRequest>(out var typeInfo))
        {
            sender.Response.MakeErrorResponse("Cannot get ConnectRequest typeInfo");
            sender.SendResponse();
            return true;
        }

        ConnectRequest? userinfo = JsonSerializer.Deserialize(sender.Request.Body, typeInfo);
        if (userinfo == null)
        {
            sender.Response.MakeErrorResponse("ConnectRequest cannot parse");
            sender.SendResponse();
            return true;
        }

        var range = SemanticVersioning.Range.Parse(MainIni.Instance.Lobby.VersionRange);
        var version = SemanticVersioning.Version.Parse(userinfo.Version);

        if (!range.IsSatisfied(version, true))
        {
            sender.Response.MakeErrorResponse("VersionCheck");
            sender.SendResponse();
            return true;
        }

        var user = MainControl.Database.GetUserDatas().FirstOrDefault(x => x.CustomId == userinfo.UserId);
        if (user == null)
        {
            user = new()
            {
                Id = Guid.NewGuid(),
                CustomId = userinfo.UserId,
                Name = userinfo.PlatformUserName,
                FriendsIds = [],
                BlockList = new([], [], []),
                FriendRequests = [],
            };
            MainControl.Database.SaveUserData(user);

            // create stash
            UserStash stash = new()
            {
                Id = user.Id,
                Stash = new(),
            };

            if (GameDataStorage.Stashes.TryGetValue(MainIni.Instance.GameData.DefaultStashName, out var out_stash))
            {
                stash.Stash = out_stash;
            }

            MainControl.Database.SaveStashInventory(stash);

            UserProfile profile = new()
            {
                Id = user.Id,
                Character = new()
                {
                    Name = user.Name,
                    CreationDate = DateTime.Now,
                    Inventory = new(),
                    Modules = [],
                    Origin = string.Empty,
                },
            };
            string origin = MainIni.Instance.GameData.DefaultOrigin;

            // Additional Datas must be "Key1=Value1;Key2=Value2".
            if (userinfo.AdditionalData.Contains("origin", StringComparison.InvariantCultureIgnoreCase))
            {
                origin = userinfo.AdditionalData.Split("origin=")[1].Split(";")[0];
            }

            profile.Character.Origin = origin;

            if (GameDataStorage.Inventories.TryGetValue(MainIni.Instance.GameData.DefaultInventoryName, out var out_inventory))
            {
                profile.Character.Inventory = out_inventory;
            }

            if (GameDataStorage.OriginToModules.TryGetValue(origin, out var out_modules))
            {
                profile.Character.Modules.Clear();
                foreach (var item in out_modules)
                {
                    profile.Character.Modules.Add(item);
                }
            }

            MainControl.Database.SaveProfile(profile);
        }

        sender.Response.MakeGetResponse(JsonSerializer.Serialize(new ConnectResponse(user.Id, string.Empty), connectResponseTypeInfo));
        sender.SendResponse();
        return true;
    }
}
