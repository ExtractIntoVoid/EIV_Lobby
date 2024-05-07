using LobbyLib.Jsons;

namespace LobbyLib.Database
{
    public interface IDatabase
    {
        public void Create();

        public void Open();

        public void Close();

        // Inventory

        public void SaveInventory(Inventory inventory);

        public Inventory? GetInventory(Guid Id);

        public void DeleteInventory(Guid Id);

        // Stash

        public void SaveStashInventory(StashInventory inventory);

        public StashInventory? GetStashInventory(Guid Id);        

        public void DeleteStashInventory(Guid Id);

        // Chat

        public void AddChat(ChatMessage chat);

        public ChatMessage? GetChat(string ReceiverId, ulong MessageId);

        public List<ChatMessage> GetChats(string ReceiverId);

        public void DeleteChat(string ReceiverId);

        // User Data

        public void SaveUserData(UserData userData);

        public UserData? GetUserData(string UserId);

        public void DeleteUserData(string UserId);

        // Badges

        public void SaveBadge(Badge badge);

        public Badge? GetBadge(string UserId);

        public void DeleteBadge(string UserId);
    }
}
