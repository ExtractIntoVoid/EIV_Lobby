using LobbyLib.Jsons;
using JsonLib.Convert;
using Newtonsoft.Json;

namespace LobbyLib.Database
{
    public class JsonDatabase : IDatabase
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

        public void SaveInventory(Inventory inventory)
        {
            Create();
            var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-","_"), "Inventory.json");
            if (!Directory.Exists(Path.GetDirectoryName(json)))
                Directory.CreateDirectory(Path.GetDirectoryName(json)!);
            File.WriteAllText(json, JsonConvert.SerializeObject(inventory, new JsonSerializerSettings()
            { 
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            }));
        }

        public Inventory? GetInventory(Guid Id)
        {
            Create();
            var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "Inventory.json");
            if (!File.Exists(json))
                return null;
            var settings = ConvertHelper.GetSerializerSettings();
            return JsonConvert.DeserializeObject<Inventory>(File.ReadAllText(json), settings);
        }


        public StashInventory? GetStashInventory(Guid Id)
        {
            Create();
            var json = Path.Combine(dir_path, Id.ToString().Replace("-", "_"), "StashInventory.json");
            if (!File.Exists(json))
                return null;
            var settings = ConvertHelper.GetSerializerSettings();
            return JsonConvert.DeserializeObject<StashInventory>(File.ReadAllText(json), settings);
        }

        public void SaveStashInventory(StashInventory inventory)
        {
            Create();
            var json = Path.Combine(dir_path, inventory.UserId.ToString().Replace("-", "_"), "StashInventory.json");
            var settings = ConvertHelper.GetSerializerSettings();
            File.WriteAllText(json, JsonConvert.SerializeObject(inventory, settings));
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


        public void AddChat(ChatMessage chat)
        {
            var messages = GetChats(chat.ReceiverId);
            messages.Add(chat);
            var json = Path.Combine(dir_path, chat.ReceiverId.ToString().Replace("@","_"), "ChatMessages.json");
            File.WriteAllText(json, JsonConvert.SerializeObject(messages));
        }

        public List<ChatMessage> GetChats(string ReceiverId)
        {
            var json = Path.Combine(dir_path, ReceiverId.ToString().Replace("@", "_"), "ChatMessages.json");
            return JsonConvert.DeserializeObject<List<ChatMessage>>(File.ReadAllText(json)) ?? new();
        }

        public ChatMessage? GetChat(string ReceiverId, ulong MessageId)
        {
            var messages = GetChats(ReceiverId);
            return messages.Where(x => x.Id == MessageId).FirstOrDefault();
        }

        public void DeleteChat(string ReceiverId)
        {
            var json = Path.Combine(dir_path, ReceiverId.ToString().Replace("@", "_"), "StashInventory.json");
            if (File.Exists(json))
                File.Delete(json);
        }
    }
}
