using System.Collections.ObjectModel;

namespace EIV_LobbyData.Pipe;

/// <summary>
/// Represent a map sync on the pipe.
/// </summary>
/// <param name="Maps">The maps from game.</param>
public record class MapSync(Collection<MapData> Maps) : IPipeMessage;
