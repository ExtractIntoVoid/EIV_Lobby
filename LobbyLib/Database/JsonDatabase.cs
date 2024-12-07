using LobbyLib.Jsons;
using EIV_JsonLib.Json;
using System.Text.Json;

namespace LobbyLib.Database;

internal class JsonDatabase : IDatabase
{
    public string dir_path = Path.Combine(Directory.GetCurrentDirectory(), "Database", "JsonDatabase");
    public void Create()
    {
        if (!Directory.Exists(dir_path))
            Directory.CreateDirectory(dir_path);
    }
    public static void CreateDir(string dir)
    {
        var dirname = Path.GetDirectoryName(dir);
        if (!string.IsNullOrEmpty(dirname) && !Directory.Exists(dirname))
            Directory.CreateDirectory(dirname);
    }
    #region Inventory
    public void SaveInventory(UserInventory inventory)
    {
        Create();
        var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-", "_"), "Inventory.json");
        CreateDir(json);
        File.WriteAllText(json, JsonSerializer.Serialize(inventory, ConvertHelper.GetSerializerSettings()));
    }

    public UserInventory? GetInventory(Guid Id)
    {
        Create();
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "Inventory.json");
        if (!File.Exists(json))
            return null;
        return JsonSerializer.Deserialize<UserInventory>(File.ReadAllText(json), ConvertHelper.GetSerializerSettings());
    }

    public void DeleteInventory(Guid Id)
    {
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "Inventory.json");
        if (File.Exists(json))
            File.Delete(json);
    }
    #endregion
    #region Stash
    public void SaveStashInventory(StashInventory inventory)
    {
        Create();
        var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-", "_"), "StashInventory.json");
        CreateDir(json);
        File.WriteAllText(json, JsonSerializer.Serialize(inventory, ConvertHelper.GetSerializerSettings()));
    }

    public StashInventory? GetStashInventory(Guid Id)
    {
        Create();
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "StashInventory.json");
        if (!File.Exists(json))
            return null;
        return JsonSerializer.Deserialize<StashInventory>(File.ReadAllText(json), ConvertHelper.GetSerializerSettings());
    }

    public void DeleteStashInventory(Guid Id)
    {
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "StashInventory.json");
        if (File.Exists(json))
            File.Delete(json);
    }
    #endregion
    #region UserData
    public void SaveUserData(UserData userData)
    {
        Create();
        var json = Path.Combine(dir_path, userData.Id.ToString().Replace("-", "_"), "UserData.json");
        CreateDir(json);
        File.WriteAllText(json, JsonSerializer.Serialize(userData));
    }

    public List<UserData> GetUserDatas()
    {
        Create();
        List<UserData> userDatas = [];
        foreach (string userId in Directory.GetDirectories(dir_path))
        {
            // for log
            Console.WriteLine(userId);
            var userDataPath = Path.Combine(userId, "UserData.json");
            if (File.Exists(userDataPath))
            {
                userDatas.Add(JsonSerializer.Deserialize<UserData>(File.ReadAllText(userDataPath))!);
            }
        }

        return userDatas;
    }

    public UserData? GetUserData(Guid Id)
    {
        Create();
        var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "UserData.json");
        if (!File.Exists(json))
            return null;
        return JsonSerializer.Deserialize<UserData>(File.ReadAllText(json));
    }

    public void DeleteUserData(Guid Id)
    {
        var json = Path.Combine(dir_path, Id.ToString().Replace("@", "_"), "UserData.json");
        if (File.Exists(json))
            File.Delete(json);
    }
    #endregion
}
