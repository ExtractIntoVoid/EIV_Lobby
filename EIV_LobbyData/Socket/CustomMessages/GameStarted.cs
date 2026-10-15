namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent a message when a game started.
/// </summary>
/// <param name="Connection">The connection to join.</param>
public record class GameStarted(string Connection) : ICustomMessage;
