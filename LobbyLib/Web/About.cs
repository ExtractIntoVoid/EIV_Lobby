using EIV_LobbyData;
using LobbyLib.INI;
using ModdableWebServer.Attributes;
using ModdableWebServer.Senders;
using System.Text.Json;

namespace LobbyLib.Web;

/// <summary>
/// Represent a Lobby Web communication.
/// </summary>
internal static partial class LobbyWeb
{
    /// <summary>
    /// <see cref="About(ServerSender)"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <returns>ToSkip.</returns>
    [HTTP("GET", "/About")]
    public static bool About(ServerSender sender)
    {
        AboutResponse response = new(
            [],
            new GameInfo(MainIni.Instance.Lobby.VersionRange, []),
            new LobbyInfo(
                MainIni.Instance.Lobby.Name,
                MainIni.Instance.Lobby.Description,
                MainIni.Instance.Lobby.ReadLongDescription(),
                ChatUserToWS.Count,
                MainIni.Instance.Lobby.MaxPlayers));

        if (!JsonSerializerConvert.TryGetTypeInfo<AboutResponse>(out var typeInfo))
        {
            sender.Response.MakeErrorResponse();
            sender.SendResponse();
            return true;
        }

        sender.Response.MakeGetResponse(JsonSerializer.Serialize(response, typeInfo));
        sender.SendResponse();
        return true;
    }
}
