using System.Text.Json.Serialization;

namespace EIV_LobbyData.Socket.CustomMessages;

/// <summary>
/// Represent an interface for custom socket message.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
#pragma warning disable CA1040 // Avoid empty interfaces
public interface ICustomMessage;
#pragma warning restore CA1040 // Avoid empty interfaces
