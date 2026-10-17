using System.Text.Json.Serialization;

namespace EIV_LobbyData.Pipe;

/// <summary>
/// Represent an interface for custom pipe message.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IPipeMessage;
#pragma warning restore CA1040 // Avoid empty interfaces