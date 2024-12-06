using LobbyLib.Jsons;

namespace LobbyLib.Database;

internal class LiteDB_Database : IDatabase
{

    public void Create()
    {
        
    }

    public void DeleteInventory(Guid Id)
    {
        
    }

    public void DeleteStashInventory(Guid Id)
    {
        
    }

    public void DeleteUserData(Guid Id)
    {
        
    }
    public UserInventory? GetInventory(Guid Id)
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


    public void SaveInventory(UserInventory inventory)
    {
        
    }

    public void SaveStashInventory(StashInventory inventory)
    {
        
    }

    public void SaveUserData(UserData userData)
    {
        
    }
}
