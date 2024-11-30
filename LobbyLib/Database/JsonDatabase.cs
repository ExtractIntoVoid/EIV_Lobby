using LobbyLib.Jsons;
using EIV_JsonLib.Json;
using System.Text.Json;

namespace LobbyLib.Database;

internal class JsonDatabase : IDatabase
{
    public string dir_path = Path.Combine(Directory.GetCurrentDirectory(), "Database", "JsonDatabase");
    public void Create()
    {
        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Database", "JsonDatabase")))
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Database", "JsonDatabase"));
    }

    public void Open()
    {

    }
    public void Close()
    {

    }

    public void SaveInventory(UserInventory inventory)
    {
        Create();
        var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-","_"), "Inventory.json");
        if (!Directory.Exists(Path.GetDirectoryName(json)))
            Directory.CreateDirectory(Path.GetDirectoryName(json)!);
        File.WriteAllText(json, JsonSerializer.Serialize(inventory));
    }

    public UserInventory? GetInventory(Guid Id)
    {
        Create();
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "Inventory.json");
        if (!File.Exists(json))
            return null;
        var settings = ConvertHelper.GetSerializerSettings();
        return JsonSerializer.Deserialize<UserInventory>(File.ReadAllText(json), settings);
    }


    public StashInventory? GetStashInventory(Guid Id)
    {
        Create();
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "StashInventory.json");
        if (!File.Exists(json))
            return null;
        var settings = ConvertHelper.GetSerializerSettings();
        return JsonSerializer.Deserialize<StashInventory>(File.ReadAllText(json), settings);
    }

    public void SaveStashInventory(StashInventory inventory)
    {
        Create();
        var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-", "_"), "StashInventory.json");
        File.WriteAllText(json, JsonSerializer.Serialize(inventory));
    }

    public void DeleteInventory(Guid Id)
    {
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "Inventory.json");
        if (File.Exists(json))
            File.Delete(json);
    }

    public void DeleteStashInventory(Guid Id)
    {
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "StashInventory.json");
        if (File.Exists(json))
            File.Delete(json);
    }

    public void DeleteChat(string ReceiverId)
    {
        var json = Path.Combine(dir_path, ReceiverId.ToString().Replace("@", "_"), "StashInventory.json");
        if (File.Exists(json))
            File.Delete(json);
    }

    public void SaveUserData(UserData userData)
    {
        
    }

    public UserData? GetUserData(string UserId)
    {
        return null;
    }

    public void DeleteUserData(string UserId)
    {
        
    }
}
