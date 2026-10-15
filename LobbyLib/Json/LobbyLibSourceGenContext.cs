using LobbyLib.Models;
using System.Text.Json.Serialization;

namespace LobbyLib.Json;

/// <summary>
/// Repressent an lobby data source generated context.
/// </summary>
[JsonSourceGenerationOptions]
[JsonSerializable(typeof(UserStash))]
[JsonSerializable(typeof(UserData))]
[JsonSerializable(typeof(UserProfile))]
public partial class LobbyLibSourceGenContext : JsonSerializerContext
{
}