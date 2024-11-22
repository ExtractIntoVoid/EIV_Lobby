﻿using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using Newtonsoft.Json;
using LobbyLib.Jsons;
using LobbyLib.JWT;
using EIV_Common.InfoJson;

namespace LobbyLib.Web;

internal class Connections
{
    // this use if we connect with MasterServer
    [HTTP("POST", "/EIV_Lobby/Connect")]
    public static bool Connect(HttpRequest request, ServerStruct serverStruct)
    {
        // JWT Decode?

        serverStruct.Response.MakeGetResponse("");
        serverStruct.SendResponse();
        return true;
    }

    // Direct connection, not used with masterserver
    [HTTP("POST", "/EIV_Lobby/DirectConnect")]
    public static bool DirectConnect(HttpRequest request, ServerStruct serverStruct)
    {
        var userinfo = JsonConvert.DeserializeObject<UserInfoJson>(request.Body);
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

        serverStruct.Response.MakeGetResponse(JWTHelper.Create(data));
        serverStruct.SendResponse();
        return true;
    }
}
