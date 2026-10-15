using LobbyLib.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace LobbyLib.Database;

/// <summary>
/// Represent a json database.
/// </summary>
public class JsonDatabase : IDatabase
{
    /// <summary>
    /// Creates a directory.
    /// </summary>
    /// <param name="dir">The directory name.</param>
    public static void CreateDir(string dir)
    {
        var dirname = Path.GetDirectoryName(dir);
        if (!string.IsNullOrEmpty(dirname) && !Directory.Exists(dirname))
        {
            Directory.CreateDirectory(dirname);
        }
    }

    private readonly string jsonDatabasePath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "JsonDatabase");

    /// <inheritdoc/>
    public void Create()
    {
        if (!Directory.Exists(jsonDatabasePath))
        {
            Directory.CreateDirectory(jsonDatabasePath);
        }
    }

    /// <inheritdoc/>
    public void SaveProfile(UserProfile profile)
    {
        if (profile == null)
        {
            return;
        }

        Create();
        var json = Path.Combine(jsonDatabasePath, profile.Id.Simplyfy(), "Inventory.json");
        CreateDir(json);

        if (!JsonSerializerConvert.TryGetTypeInfo<UserProfile>(out var typeInfo))
        {
            return;
        }

        File.WriteAllText(json, JsonSerializer.Serialize(profile, typeInfo));
    }

    /// <inheritdoc/>
    public UserProfile? GetProfile(Guid id)
    {
        Create();
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "Inventory.json");
        if (!File.Exists(json))
        {
            return null;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<UserProfile>(out var typeInfo))
        {
            return null;
        }

        return JsonSerializer.Deserialize<UserProfile>(File.ReadAllText(json), typeInfo);
    }

    /// <inheritdoc/>
    public void DeleteProfile(Guid id)
    {
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "Inventory.json");
        if (File.Exists(json))
        {
            File.Delete(json);
        }
    }

    /// <inheritdoc/>
    public void SaveStashInventory(UserStash inventory)
    {
        if (inventory == null)
        {
            return;
        }

        Create();
        var json = Path.Combine(jsonDatabasePath, inventory.Id.Simplyfy(), "StashInventory.json");
        CreateDir(json);

        if (!JsonSerializerConvert.TryGetTypeInfo<UserStash>(out var typeInfo))
        {
            return;
        }

        File.WriteAllText(json, JsonSerializer.Serialize(inventory, typeInfo));
    }

    /// <inheritdoc/>
    public UserStash? GetStashInventory(Guid id)
    {
        Create();
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "StashInventory.json");
        if (!File.Exists(json))
        {
            return null;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<UserStash>(out var typeInfo))
        {
            return null;
        }

        return JsonSerializer.Deserialize<UserStash>(File.ReadAllText(json), typeInfo);
    }

    /// <inheritdoc/>
    public void DeleteStashInventory(Guid id)
    {
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "StashInventory.json");
        if (File.Exists(json))
        {
            File.Delete(json);
        }
    }

    /// <inheritdoc/>
    public void SaveUserData(UserData userData)
    {
        if (userData == null)
        {
            return;
        }

        Create();
        var json = Path.Combine(jsonDatabasePath, userData.Id.Simplyfy(), "UserData.json");
        CreateDir(json);

        if (!JsonSerializerConvert.TryGetTypeInfo<UserData>(out var typeInfo))
        {
            return;
        }

        File.WriteAllText(json, JsonSerializer.Serialize(userData, typeInfo));
    }

    /// <inheritdoc/>
    public Collection<UserData> GetUserDatas()
    {
        Create();
        Collection<UserData> userDatas = [];

        if (!JsonSerializerConvert.TryGetTypeInfo<UserData>(out var typeInfo))
        {
            return userDatas;
        }

        foreach (string userId in Directory.GetDirectories(jsonDatabasePath))
        {
            // for log
            Console.WriteLine(userId);
            var userDataPath = Path.Combine(userId, "UserData.json");
            if (File.Exists(userDataPath))
            {
                UserData? data = JsonSerializer.Deserialize<UserData>(File.ReadAllText(userDataPath), typeInfo);

                if (data == null)
                {
                    continue;
                }

                userDatas.Add(data);
            }
        }

        return userDatas;
    }

    /// <inheritdoc/>
    public UserData? GetUserData(Guid id)
    {
        Create();
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "UserData.json");
        if (!File.Exists(json))
        {
            return null;
        }

        if (!JsonSerializerConvert.TryGetTypeInfo<UserData>(out var typeInfo))
        {
            return null;
        }

        return JsonSerializer.Deserialize<UserData>(File.ReadAllText(json), typeInfo);
    }

    /// <inheritdoc/>
    public void DeleteUserData(Guid id)
    {
        var json = Path.Combine(jsonDatabasePath, id.Simplyfy(), "UserData.json");
        if (File.Exists(json))
        {
            File.Delete(json);
        }
    }
}
