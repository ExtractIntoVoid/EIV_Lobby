using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using EIV_Common;
using EIV_JsonLib;

namespace LobbyLib.Web;

internal class About
{
    [HTTP("GET", "/EIV_Lobby/About")]
    public static bool Connect(HttpRequest request, ServerStruct serverStruct)
    {
        ServerInfoJson serverInfoJSON = new()
        {
            GameServerInfo = new()
            {
                ServerAddress = ConfigINI.Read("Lobby.ini", "Lobby", "ServerAddress"),
                ServerPort = ConfigINI.Read<int>("Lobby.ini", "Lobby", "ServerPort"),
            },
            Game = new()
            {
                Version = ConfigINI.Read("Lobby.ini", "Lobby", "Version"),
            }

        };

        serverStruct.Response.MakeGetResponse("");
        serverStruct.SendResponse();
        return true;
    }
}
