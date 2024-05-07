using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using LobbyLib.Jsons;
using LobbyLib.JWT;

namespace LobbyLib.Web
{
    internal class Connections
    {
        // this use if we connect with MasterServer
        [HTTP("POST", "/EIV_Lobby/Connect")]
        public static bool Connect(HttpRequest request, ServerStruct serverStruct)
        {
            

            serverStruct.Response.MakeGetResponse("");
            serverStruct.SendResponse();
            return true;
        }

        // Direct connection, not used with masterserver
        [HTTP("POST", "/EIV_Lobby/DirectConnect")]
        public static bool DirectConnect(HttpRequest request, ServerStruct serverStruct)
        {
            var userinfo = JsonConvert.DeserializeObject<UserInfoJson_JWT>(request.Body);
            if (userinfo == null)
            {
                serverStruct.Response.MakeErrorResponse("UserInfoJson_JWT cannot parse");
                serverStruct.SendResponse();
                return true;
            }

            var data = MainControl.Database.GetUserData(userinfo.CreateUserId());
            if (data == null)
            {
                data = new()
                {
                    UserId = userinfo.CreateUserId(),
                    Name = userinfo.Name,
                    FriendsIds = new(),
                    RSA_PubKey_XML = string.Empty
                };
                MainControl.Database.SaveUserData(data);
            }

            if (userinfo.AdditionalData is Badge badgedata && badgedata != null)
            {
                var jwt = JWTHelper.Create(data, badgedata);
                serverStruct.Response.MakeGetResponse(jwt);
                serverStruct.SendResponse();
                return true;
            }
            string JWT = string.Empty;
            var badge = MainControl.Database.GetBadge(userinfo.CreateUserId());
            if (badge == null)
                JWT = JWTHelper.Create(data);
            else
                JWT = JWTHelper.Create(data, badge);
            serverStruct.Response.MakeGetResponse(JWT);
            serverStruct.SendResponse();
            return true;
        }
    }
}
