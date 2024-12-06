using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer;
using NetCoreServer;
using System.Text.Json;
using EIV_Common.InfoJson;
using EIV_Common;
using EIV_JsonLib.Lobby;
using LobbyLib.CustomTicket;

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

        // Simple version check. PLEASE REPLACE WITH NORMAL ONE!
        if (userinfo.Version != ConfigINI.Read("Lobby.ini", "Lobby", "Version"))
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

            // TODO: Create a new save file.
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
