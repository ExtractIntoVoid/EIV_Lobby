using LobbyLib.Managers;
using ModdableWebServer.Attributes;
using ModdableWebServer.Senders;

namespace LobbyLib.Web;

/// <summary>
/// Represent a Lobby Web communication.
/// </summary>
internal partial class LobbyWeb
{
    /// <summary>
    /// <see cref="FilesMods(ServerSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <returns>ToSkip.</returns>
#pragma warning disable CA2243 // Attribute string literals should parse correctly
    [HTTP("GET", "/Files/{build}/Mods/{modpath}")]
#pragma warning restore CA2243 // Attribute string literals should parse correctly
    public static bool FilesMods(ServerSender sender)
    {
        string build = sender.Parameters["build"];
        string modpath = sender.Parameters["modpath"];
        if (modpath == "mods.txt")
        {
            // All currently loaded mods.
            var rsp = ModDownloadManager.GetModsTxt(build);
            sender.Response.MakeGetResponse(rsp);
            sender.SendResponse();
            return true;
        }

        // get the actual files here.
        var res = ModDownloadManager.GetFile(build, modpath);
        if (res.Length == 0)
        {
            sender.Response.MakeErrorResponse("File is not found!");
            sender.SendResponse();
            return true;
        }

        sender.Response.MakeGetResponse(res);
        sender.SendResponse();
        return true;
    }
}
