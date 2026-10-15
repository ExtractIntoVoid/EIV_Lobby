using EIV_LobbyData;
using LobbyLib.INI;
using LobbyLib.Models;
using System.Diagnostics.CodeAnalysis;

namespace LobbyLib.Managers;

/// <summary>
/// Provides stash management.
/// </summary>
public static class StashManager
{
    /// <summary>
    /// Gets the stash from ticket.
    /// </summary>
    /// <param name="ticket">The ticket.</param>
    /// <returns>The stash.</returns>
    public static UserStash GetStash([NotNull] TicketModel ticket)
    {
        var stash = MainControl.Database.GetStashInventory(ticket.Id);
        if (stash == null)
        {
            CreateStash(ref stash, ticket.Id);
        }

        return stash;
    }

    /// <summary>
    /// Creates a stash profile.
    /// </summary>
    /// <param name="stash">The profile.</param>
    /// <param name="id">The user id.</param>
    public static void CreateStash([NotNull] ref UserStash? stash, Guid id)
    {
        stash = new()
        {
            Id = id,
            Stash = new(),
        };

        if (GameDataStorage.Stashes.TryGetValue(MainIni.Instance.GameData.DefaultStashName, out var out_stash))
        {
            stash.Stash = out_stash;
        }

        MainControl.Database.SaveStashInventory(stash);
    }
}
