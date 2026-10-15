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
    /// <see cref="ProfileStash(ServerSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <returns>ToSkip.</returns>
    [HTTP("GET", "/Profile/Stash")]
    public static bool ProfileStash(ServerSender sender)
    {
        if (!sender.Headers.TryGetValue("authorization", out string? ticket))
        {
            sender.Response.MakeErrorResponse(401, "Authorization is not found!");
            sender.SendResponse();
            return true;
        }

        var ticketstruct = TicketManager.GetTicket(ticket);
        if (ticketstruct == null)
        {
            sender.Response.MakeErrorResponse(401, "Wrong ticket!");
            sender.SendResponse();
            return true;
        }

        var stash = StashManager.GetStash(ticketstruct);

        sender.Response.MakeGetResponse(stash.Stash.Serialize());
        sender.SendResponse();
        return true;
    }
}