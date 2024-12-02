using ModdableWebServer.Attributes;
using ModdableWebServer;
using NetCoreServer;
using ModdableWebServer.Helper;
using LobbyLib.Modding;

namespace LobbyLib.Web;

internal partial class EIV_Lobby
{
    [HTTP("GET", "/EIV_Lobby/Files/Client/Mods/{modpath}")]
    public static bool ClientMods(HttpRequest request, ServerStruct serverStruct)
    {
        string modpath = serverStruct.Parameters["modpath"];
        if (modpath == "mods.txt")
        {
            var rsp = string.Join("\n",ModLoader.ModsFiles.Keys);
            serverStruct.Response.MakeGetResponse(rsp);
            serverStruct.SendResponse();
            return true;
        }

        if (modpath.Contains("mods.txt"))
        {
            modpath = modpath.Replace(".mods.txt", "");
            //modlist from that replace first _ to . and you get the soltion
            if (modpath.Contains("_"))
            {
                modpath = modpath.Replace("_",".");
            }
            Console.WriteLine(modpath);
            if (ModLoader.ModsFiles.TryGetValue(modpath, out var mod))
            {
                var rsp = string.Join("\n", mod);
                serverStruct.Response.MakeGetResponse(rsp);
                serverStruct.SendResponse();
                return true;
            }
            else
            {
                serverStruct.Response.MakeErrorResponse();
                serverStruct.SendResponse();
                return true;
            }
        }

        //get the actual files here.
        


        serverStruct.Response.MakeGetResponse("");
        serverStruct.SendResponse();
        return true;
    }
}
