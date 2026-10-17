namespace EIV_LobbyData.Pipe;

/// <summary>
/// Represent a map data.
/// </summary>
/// <param name="Name">The name of the map.</param>
/// <param name="MinPlayers">The minimum players can be on the map.</param>
/// <param name="MaxPlayers">The maxiumum players can be on the map.</param>
public record class MapData(string Name, int MinPlayers, int MaxPlayers);