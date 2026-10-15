using System.Collections.ObjectModel;

namespace EIV_LobbyData;

/// <summary>
/// Represent an "/about" page response.
/// </summary>
/// <param name="Downloadables">The downloadables for the game.</param>
/// <param name="Game">The game information.</param>
/// <param name="Lobby">The lobby information.</param>
public record class AboutResponse(Collection<DownloadInfo> Downloadables, GameInfo Game, LobbyInfo Lobby);