using System.Collections.ObjectModel;

namespace EIV_LobbyData;

/// <summary>
/// Represent a game related information.
/// </summary>
/// <param name="Version">The game version.</param>
/// <param name="AvailableMaps">The maps available in the game to choose from.</param>
public record class GameInfo(string Version, Collection<string> AvailableMaps);