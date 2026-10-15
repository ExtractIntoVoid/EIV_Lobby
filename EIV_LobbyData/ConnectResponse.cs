namespace EIV_LobbyData;

/// <summary>
/// Represent a "/connect" response.
/// </summary>
/// <param name="Id">The user id.</param>
/// <param name="Ticket">The user ticket.</param>
public record class ConnectResponse(Guid Id, string Ticket);