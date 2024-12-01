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
            Game = new()
            {
                Version = ConfigINI.Read("Lobby.ini", "Lobby", "Version"),
            },
            Server = new()
            { 
                ServerDescription = ConfigINI.Read("Lobby.ini", "Lobby", "Name"),
                ServerName = ConfigINI.Read("Lobby.ini", "Lobby", "Description"),

            }

        };

        serverStruct.Response.MakeGetResponse("");
        serverStruct.SendResponse();
        return true;
    }
}
