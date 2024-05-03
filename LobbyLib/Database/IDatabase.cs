using LobbyLib.Jsons;

namespace LobbyLib.Database
{
    public interface IDatabase
    {
        public void Create();

        public void Open();

        public void Close();

        public void SaveInventory(Inventory inventory);

        public Inventory? GetInventory(Guid Id);

        public void DeleteInventory(Guid Id);

        public void SaveStashInventory(StashInventory inventory);

        public StashInventory? GetStashInventory(Guid Id);        

        public void DeleteStashInventory(Guid Id);

        // Other Database operation I guess
        public void AddChat(ChatMessage chat);

        public ChatMessage? GetChat(string ReceiverId, ulong MessageId);

        public List<ChatMessage> GetChats(string ReceiverId);

        public void DeleteChat(string ReceiverId);
    }
}
