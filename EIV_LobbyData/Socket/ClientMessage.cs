using EIV_LobbyData.Socket.CustomMessages;

namespace EIV_LobbyData.Socket;

/// <summary>
/// Represent an "/Socket/Client" page communication.
/// </summary>
/// <param name="Message">The message to sent.</param>
public record class ClientMessage(ICustomMessage Message);