using LobbyLib.Jsons;

namespace LobbyLib.Database;

internal class EmptyDatabase : IDatabase
{
    public void Close()
    {

    }

    public void Create()
    {
        
    }

    public void DeleteInventory(Guid Id)
    {
        
    }

    public void DeleteStashInventory(Guid Id)
    {
        
    }

    public void DeleteUserData(string UserId)
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

    public UserData? GetUserData(string UserId)
    {
        return null;
    }

    public void Open()
    {
        
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
