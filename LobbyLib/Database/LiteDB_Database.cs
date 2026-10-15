using LobbyLib.Models;
using System.Collections.ObjectModel;

namespace LobbyLib.Database;

/// <summary>
/// Represent an liteDB database.
/// </summary>
public class LiteDB_Database : IDatabase
{
    /// <inheritdoc/>
    public void Create()
    {
    }

    /// <inheritdoc/>
    public void DeleteProfile(Guid id)
    {
    }

    /// <inheritdoc/>
    public void DeleteStashInventory(Guid id)
    {
    }

    /// <inheritdoc/>
    public void DeleteUserData(Guid id)
    {
    }

    /// <inheritdoc/>
    public UserProfile? GetProfile(Guid id) => null;

    /// <inheritdoc/>
    public UserStash? GetStashInventory(Guid id) => null;

    /// <inheritdoc/>
    public UserData? GetUserData(Guid id) => null;

    /// <inheritdoc/>
    public Collection<UserData> GetUserDatas() => [];

    /// <inheritdoc/>
    public void SaveProfile(UserProfile profile)
    {
    }

    /// <inheritdoc/>
    public void SaveStashInventory(UserStash inventory)
    {
    }

    /// <inheritdoc/>
    public void SaveUserData(UserData userData)
    {
    }
}
