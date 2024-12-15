using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using ModAPI;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    [HTTP("GET", "/EIV_Lobby/Files/{build}/Mods/{modpath}")]
    public static bool FilesMods(HttpRequest request, ServerStruct serverStruct)
    {
        string modpath = serverStruct.Parameters["modpath"];
        if (modpath == "mods.txt")
        {
            // All currently loaded mods.
            var rsp = string.Join("\n", MainLoader.Mods.Select(x=>x.FullName));
            serverStruct.Response.MakeGetResponse(rsp);
            serverStruct.SendResponse();
            return true;
        }
        //get the actual files here.
        


        serverStruct.Response.MakeGetResponse("");
        serverStruct.SendResponse();
        return true;
    }
}
