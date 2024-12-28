using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using LobbyLib.Managers;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{

    [HTTP("GET", "/Files/{build}/Mods/{modpath}")]
    public static bool FilesMods(HttpRequest request, ServerStruct serverStruct)
    {
        string build = serverStruct.Parameters["build"];
        string modpath = serverStruct.Parameters["modpath"];
        if (modpath == "mods.txt")
        {
            // All currently loaded mods.
            var rsp = ModDownloadManager.GetModsTxt(build);
            serverStruct.Response.MakeGetResponse(rsp);
            serverStruct.SendResponse();
            return true;
        }
        //get the actual files here.
        var res = ModDownloadManager.GetFile(build, modpath);
        if (res.Length == 0)
        {
            serverStruct.Response.MakeErrorResponse("File is not found!");
            serverStruct.SendResponse();
            return true;
        }
        serverStruct.Response.MakeGetResponse(res);
        serverStruct.SendResponse();
        return true;
    }
}
