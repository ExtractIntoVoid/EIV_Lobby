namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a matchmake check for <paramref name="Map"/>.
/// </summary>
/// <param name="Map">The map to check.</param>
public record class MatchmakeCheck(string Map) : ICustomMessage;
