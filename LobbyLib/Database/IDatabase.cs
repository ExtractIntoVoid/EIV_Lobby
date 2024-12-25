using LobbyLib.Jsons;

namespace LobbyLib.Database;

public interface IDatabase
{
    public void Create();

    // Profile
    public void SaveProfile(UserProfile profile);
    public UserProfile? GetProfile(Guid Id);
    public void DeleteProfile(Guid Id);

    // Stash

    public void SaveStashInventory(StashInventory inventory);
    public StashInventory? GetStashInventory(Guid Id);        
    public void DeleteStashInventory(Guid Id);

    // User Data

    public void SaveUserData(UserData userData);
    public List<UserData> GetUserDatas();
    public UserData? GetUserData(Guid Id);
    public void DeleteUserData(Guid Id);
}
