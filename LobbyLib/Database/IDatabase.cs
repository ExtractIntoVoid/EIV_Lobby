using LobbyLib.Models;
using System.Collections.ObjectModel;

namespace LobbyLib.Database;

/// <summary>
/// An interface interacting with a database.
/// </summary>
public interface IDatabase
{
    /// <summary>
    /// Create the database.
    /// </summary>
    public void Create();

    /// <summary>
    /// Saves the profile.
    /// </summary>
    /// <param name="profile">The profile.</param>
    public void SaveProfile(UserProfile profile);

    /// <summary>
    /// Gets the profile from <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns>The profile.</returns>
    public UserProfile? GetProfile(Guid id);

    /// <summary>
    /// Deletes the profile.
    /// </summary>
    /// <param name="id">The user id.</param>
    public void DeleteProfile(Guid id);

    /// <summary>
    /// Saves the stash.
    /// </summary>
    /// <param name="inventory">The stash.</param>
    public void SaveStashInventory(UserStash inventory);

    /// <summary>
    /// Gets the stash from <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns>The stash.</returns>
    public UserStash? GetStashInventory(Guid id);

    /// <summary>
    /// Deletes the stash.
    /// </summary>
    /// <param name="id">The user id.</param>
    public void DeleteStashInventory(Guid id);

    /// <summary>
    /// Saves the user data.
    /// </summary>
    /// <param name="userData">The data.</param>
    public void SaveUserData(UserData userData);

    /// <summary>
    /// Gets all saved user data.
    /// </summary>
    /// <returns>The user datas.</returns>
    public Collection<UserData> GetUserDatas();

    /// <summary>
    /// Gets the user data.
    /// </summary>
    /// <param name="id">The user id.</param>
    /// <returns>The user data.</returns>
    public UserData? GetUserData(Guid id);

    /// <summary>
    /// Deletes the user data.
    /// </summary>
    /// <param name="id">The user id.</param>
    public void DeleteUserData(Guid id);
}
