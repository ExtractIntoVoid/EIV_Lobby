using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;

namespace LobbyLib.Web
{
    internal class Connections
    {
        [HTTP("POST", "/EIV_Lobby/Connect")]
        public static bool Connect(HttpRequest request, ServerStruct serverStruct)
        {

            serverStruct.Response.MakeGetResponse("");
            serverStruct.SendResponse();
            return true;
        }

        [HTTP("POST", "/EIV_Lobby/DirectConnect")]
        public static bool DirectConnect(HttpRequest request, ServerStruct serverStruct)
        {

            serverStruct.Response.MakeGetResponse("jwt");
            serverStruct.SendResponse();
            return true;
        }
    }
}
