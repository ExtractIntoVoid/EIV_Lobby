using EIV_LobbyData;
using LobbyLib.INI;
using LobbyLib.Models;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.Managers;

/// <summary>
/// Provides a profile management.
/// </summary>
public static class ProfileManager
{
    /// <summary>
    /// Gets the profile from ticket.
    /// </summary>
    /// <param name="ticket">The ticket.</param>
    /// <returns>The profile.</returns>
    public static UserProfile GetProfile([NotNull] TicketModel ticket)
    {
        var profile = MainControl.Database.GetProfile(ticket.Id);
        if (profile == null)
        {
            CreateUserProfile(ref profile, ticket.Id, ticket.Name);
        }

        return profile;
    }

    /// <summary>
    /// Creates a user profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    /// <param name="id">The user id.</param>
    /// <param name="name">The character name.</param>
    public static void CreateUserProfile([NotNull] ref UserProfile? profile, Guid id, string name)
    {
        profile = new()
        {
            Id = id,
            Character = new()
            {
                Name = name,
                CreationDate = DateTime.Now,
                Inventory = new(),
                Modules = [],
                Origin = string.Empty,
            },
        };

        if (GameDataStorage.Inventories.TryGetValue(MainIni.Instance.GameData.DefaultInventoryName, out var out_inventory))
        {
            profile.Character.Inventory = out_inventory;
        }

        string origin = MainIni.Instance.GameData.DefaultOrigin;
        profile.Character.Origin = origin;

        if (GameDataStorage.OriginToModules.TryGetValue(origin, out var out_modules))
        {
            profile.Character.Modules.Clear();
            foreach (var item in out_modules)
            {
                profile.Character.Modules.Add(item);
            }
        }

        MainControl.Database.SaveProfile(profile);
    }
}
