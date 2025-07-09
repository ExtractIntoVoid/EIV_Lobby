using LobbyLib.Models;

namespace LobbyLib.Database;

internal class LiteDB_Database : IDatabase
{

    public void Create()
    {
        
    }

    public void DeleteProfile(Guid Id)
    {
        
    }

    public void DeleteStashInventory(Guid Id)
    {
        
    }

    public void DeleteUserData(Guid Id)
    {
        
    }
    public UserProfile? GetProfile(Guid Id)
    {
        return null;
    }

    public StashInventory? GetStashInventory(Guid Id)
    {
        return null;
    }

    public UserData? GetUserData(Guid Id)
    {
        return null;
    }
    public List<UserData> GetUserDatas()
    {
        return [];
    }


    public void SaveProfile(UserProfile inventory)
    {
        
    }

    public void SaveStashInventory(StashInventory inventory)
    {
        
    }

    public void SaveUserData(UserData userData)
    {
        
    }
}
