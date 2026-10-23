using System.Text.Json.Serialization;

namespace EIV_LobbyData.Socket.S2SMessages;

/// <summary>
/// Represent an interface for server 2 server message.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IS2SMessage;
#pragma warning restore CA1040 // Avoid empty interfaces
