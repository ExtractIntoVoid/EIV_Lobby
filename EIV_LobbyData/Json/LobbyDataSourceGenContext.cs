using EIV_LobbyData.Socket;
using EIV_LobbyData.Socket.CustomMessages;
using System.Text.Json.Serialization;

namespace EIV_LobbyData.Json;

/// <summary>
/// Repressent an lobby data source generated context.
/// </summary>
[JsonSourceGenerationOptions]
[JsonSerializable(typeof(TicketModel))]

[JsonSerializable(typeof(AboutResponse))]
[JsonSerializable(typeof(ConnectRequest))]
[JsonSerializable(typeof(ConnectResponse))]
[JsonSerializable(typeof(ChatMessage))]
[JsonSerializable(typeof(ClientMessage))]
[JsonSerializable(typeof(ClientResponse))]

[JsonSerializable(typeof(ICustomMessage))]

[JsonSerializable(typeof(FriendAction))]
[JsonSerializable(typeof(FriendAddPlatform))]
[JsonSerializable(typeof(FriendSearchRequest))]
[JsonSerializable(typeof(FriendSearchResponse))]
[JsonSerializable(typeof(GameStarted))]
[JsonSerializable(typeof(MatchmakeCheck))]
[JsonSerializable(typeof(UserBlockListUpdate))]
public partial class LobbyDataSourceGenContext : JsonSerializerContext
{
}