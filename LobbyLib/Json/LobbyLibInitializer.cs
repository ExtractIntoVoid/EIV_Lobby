namespace LobbyLib.Json;

/// <summary>
/// Provides json polymorphism register.
/// </summary>
public static class LobbyLibInitializer
{
    /// <summary>
    /// Initialize core polymorphisms and resolvers.
    /// </summary>
    public static void Initialize()
    {
        JsonSerializerConvert.Resolvers.Add(LobbyLibSourceGenContext.Default);
    }
}
