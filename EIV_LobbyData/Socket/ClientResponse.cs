namespace EIV_LobbyData.Socket;

/// <summary>
/// Represent an "/Socket/Client" page response.
/// </summary>
/// <param name="IsSuccess">Whether the message was success.</param>
/// <param name="ErrorCode">The error code.</param>
/// <param name="Message">The error message.</param>
public record class ClientResponse(bool IsSuccess, uint ErrorCode, string Message);