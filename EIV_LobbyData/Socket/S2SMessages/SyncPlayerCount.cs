namespace EIV_LobbyData.Socket.S2SMessages;

/// <summary>
/// Represent a sync player count message.
/// </summary>
/// <param name="Count">The count.</param>
public record class SyncPlayerCount(int Count) : IS2SMessage;
