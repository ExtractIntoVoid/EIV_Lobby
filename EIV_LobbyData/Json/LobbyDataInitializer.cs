using EIV_LobbyData.Pipe;
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
    /// Gets the <see cref="IPipeMessage"/> polymorphism registry.
    /// </summary>
    public static CustomPolymorphismRegistry<IPipeMessage> IPipeMessagePolymorphismRegistry { get; } = new() { Dicriminator = "Type" };

    /// <summary>
    /// Initialize core polymorphisms and resolvers.
    /// </summary>
    public static void Initialize()
    {
        JsonSerializerConvert.Resolvers.Add(LobbyDataSourceGenContext.Default);
        ICustomMessagePolymorphismRegistry.Register<MatchmakeCheck>(nameof(MatchmakeCheck));
        ICustomMessagePolymorphismRegistry.Register<GameStarted>(nameof(GameStarted));
        ICustomMessagePolymorphismRegistry.Register<FriendAction>(nameof(FriendAction));
        ICustomMessagePolymorphismRegistry.Register<FriendAddPlatform>(nameof(FriendAddPlatform));
        ICustomMessagePolymorphismRegistry.Register<FriendSearchRequest>(nameof(FriendSearchRequest));
        ICustomMessagePolymorphismRegistry.Register<FriendSearchResponse>(nameof(FriendSearchResponse));
        ICustomMessagePolymorphismRegistry.Register<UserBlockListUpdate>(nameof(UserBlockListUpdate));

        JsonSerializerConvert.Polymorphisms.Add(ICustomMessagePolymorphismRegistry);

        IPipeMessagePolymorphismRegistry.Register<Command>(nameof(Command));
        IPipeMessagePolymorphismRegistry.Register<KeepAlive>(nameof(KeepAlive));
        IPipeMessagePolymorphismRegistry.Register<MapSync>(nameof(MapSync));

        JsonSerializerConvert.Polymorphisms.Add(IPipeMessagePolymorphismRegistry);
    }
}
