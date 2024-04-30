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

        public void SaveStashInventory(StashInventory inventory);

        public StashInventory? GetStashInventory(Guid Id);

        // Other Database operation I guess
    }
}
