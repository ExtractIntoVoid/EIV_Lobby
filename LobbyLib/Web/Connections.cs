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

internal class Connections
{
    
    [HTTP("POST", "/EIV_Lobby/Connect")]
    public static bool Connect(HttpRequest request, ServerStruct serverStruct)
    {
        var userinfo = JsonSerializer.Deserialize<UserInfoJson>(request.Body);
        if (userinfo == null)
        {
            serverStruct.Response.MakeErrorResponse("UserInfoJson_JWT cannot parse");
            serverStruct.SendResponse();
            return true;
        }

        // Simple version check. PLEASE REPLACE WITH NORMAL ONE!
        if (userinfo.Version != ConfigINI.Read("Lobby.ini", "Lobby", "Version"))
        {
            serverStruct.Response.MakeErrorResponse();
            serverStruct.SendResponse();
            return true;
        }

        var data = MainControl.Database.GetUserData(userinfo.CreateUserId());
        if (data == null)
        {
            data = new()
            {
                Id = Guid.NewGuid(),
                UserId = userinfo.CreateUserId(),
                Name = userinfo.Name,
                FriendsIds = [],
                BlockList = new(),
                FriendRequests = [],
            };
            MainControl.Database.SaveUserData(data);
        }
        var ticket = TicketProcess.CreateTicket(data);

        ConnectResponse connectResponse = new()
        { 
            Id = data.Id,
            Ticket = ticket,
        };

        serverStruct.Response.MakeGetResponse(JsonSerializer.Serialize(connectResponse));
        serverStruct.SendResponse();
        return true;
    }
}
