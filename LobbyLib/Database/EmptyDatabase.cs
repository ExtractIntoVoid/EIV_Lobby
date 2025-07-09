using LobbyLib.Models;

namespace LobbyLib.Database;

internal class EmptyDatabase : IDatabase
{
    public void Create() { }
    public void DeleteProfile(Guid _) { }
    public void DeleteStashInventory(Guid _) { }
    public void DeleteUserData(Guid _) { }
    public UserProfile? GetProfile(Guid _) => null;
    public StashInventory? GetStashInventory(Guid _) => null;
    public UserData? GetUserData(Guid _) => null;
    public List<UserData> GetUserDatas() => [];
    public void SaveProfile(UserProfile _) { }
    public void SaveStashInventory(StashInventory _) { }
    public void SaveUserData(UserData _) { }
}
