using EIV_Effects;
using EIV_Items;
using EIV_Items.Core;
using EIV_Items.Json;
using EIV_LobbyData.Json;
using EIV_Profile;
using EIV_Profile.Json;
using EIV_Profile.ProfileModules;
using LobbyLib.Json;
using System.Collections.ObjectModel;

namespace LobbyLib.Managers;

/// <summary>
/// Centralized in-memory registries for game data: items, effects, stashes, inventories, and origin-to-module mappings.
/// </summary>
public static class GameDataStorage
{
    /// <summary>
    /// Gets the supported maps.
    /// </summary>
    public static readonly Collection<string> Maps = [];

    /// <summary>
    /// The available items.
    /// </summary>
    public static readonly Dictionary<string, CoreItem> Items = [];

    /// <summary>
    /// The available effects.
    /// </summary>
    public static readonly Dictionary<string, Effect> Effects = [];

    /// <summary>
    /// The available stashes.
    /// </summary>
    public static readonly Dictionary<string, Stash> Stashes = [];

    /// <summary>
    /// The available inventories.
    /// </summary>
    public static readonly Dictionary<string, Inventory> Inventories = [];

    /// <summary>
    /// The available origin to modules.
    /// </summary>
    public static readonly Dictionary<string, List<IProfileModule>> OriginToModules = [];

    /// <summary>
    /// Init formatters/serializers.
    /// </summary>
    public static void Init()
    {
        FormatterInitializer.RegisterFormatter();

        ItemJsonInitializer.Initialize();
        ProfileJsonInitializer.Initialize();
        LobbyDataInitializer.Initialize();
        LobbyLibInitializer.Initialize();
    }

    /// <summary>
    /// Loads all data.
    /// </summary>
    public static void Load()
    {
    }

    /// <summary>
    /// Clears all data.
    /// </summary>
    public static void ClearAll()
    {
        Items.Clear();
        Effects.Clear();
        Stashes.Clear();
        Inventories.Clear();
        OriginToModules.Clear();
    }
}
