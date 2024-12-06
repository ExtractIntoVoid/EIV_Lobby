using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using EIV_Common;
using EIV_JsonLib;
using System.Text.Json;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    [HTTP("GET", "/EIV_Lobby/About")]
    public static bool LobbyAbout(HttpRequest _, ServerStruct serverStruct)
    {
        ServerInfoJson serverInfoJSON = new()
        {
            Game = new()
            {
                Version = ConfigINI.Read("Lobby.ini", "Lobby", "Version"),
                AvailableMaps = []
            },
            LobbyInfo = new()
            { 
                Name = ConfigINI.Read("Lobby.ini", "Lobby", "Name"),
                Description = ConfigINI.Read("Lobby.ini", "Lobby", "Description"),
                LongDescription = ConfigINI.Read("Lobby.ini", "Lobby", "LongDescription"),
                MaxPlayerNumbers = ConfigINI.Read<int>("Lobby.ini", "Lobby", "MaxPlayers"),
                PlayerNumbers = ChatUserToWS.Count
            },
            Mods = []
        };

        serverStruct.Response.MakeGetResponse(JsonSerializer.Serialize(serverInfoJSON));
        serverStruct.SendResponse();
        return true;
    }
}
