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

        // User Data

        public void SaveUserData(UserData userData);

        public UserData? GetUserData(string UserId);

        public void DeleteUserData(string UserId);
    }
}
