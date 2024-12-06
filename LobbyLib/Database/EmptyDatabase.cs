using LobbyLib.Jsons;

namespace LobbyLib.Database;

internal class EmptyDatabase : IDatabase
{
    public void Create() { }
    public void DeleteInventory(Guid _) { }
    public void DeleteStashInventory(Guid _) { }
    public void DeleteUserData(Guid _) { }
    public UserInventory? GetInventory(Guid _) => null;
    public StashInventory? GetStashInventory(Guid _) => null;
    public UserData? GetUserData(Guid _) => null;
    public List<UserData> GetUserDatas() => [];
    public void SaveInventory(UserInventory _) { }
    public void SaveStashInventory(StashInventory _) { }
    public void SaveUserData(UserData _) { }
}
