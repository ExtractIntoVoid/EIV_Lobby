using EIV_LobbyData.Socket.CustomMessages;

namespace EIV_LobbyData.Json;

/// <summary>
/// Provides json polymorphism register.
/// </summary>
public static class LobbyDataInitializer
{
    /// <summary>
     /// Gets the <see cref="ICustomMessage"/> polymorphism registry.
     /// </summary>
    public static CustomPolymorphismRegistry<ICustomMessage> ICustomMessagePolymorphismRegistry { get; } = new() { Dicriminator = "Type" };

    /// <summary>
    /// Initialize core polymorphisms and resolvers.
    /// </summary>
    public static void Initialize()
    {
        JsonSerializerConvert.Resolvers.Add(LobbyDataSourceGenContext.Default);
        ICustomMessagePolymorphismRegistry.Register<MatchmakeCheck>(nameof(MatchmakeCheck));
        ICustomMessagePolymorphismRegistry.Register<GameStarted>(nameof(GameStarted));

        JsonSerializerConvert.Polymorphisms.Add(ICustomMessagePolymorphismRegistry);
    }
}
