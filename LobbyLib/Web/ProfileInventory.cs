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
    /// <see cref="ProfileInventory(ServerSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <returns>ToSkip.</returns>
    [HTTP("GET", "/Profile/Inventory")]
    public static bool ProfileInventory(ServerSender sender)
    {
        if (!sender.Headers.TryGetValue("authorization", out var ticket))
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

        var profile = ProfileManager.GetProfile(ticketstruct);

        sender.Response.MakeGetResponse(profile.Character.Inventory.Serialize());
        sender.SendResponse();
        return true;
    }
}