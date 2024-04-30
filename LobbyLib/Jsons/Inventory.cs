using JsonLib.Interfaces;
using Newtonsoft.Json;

namespace LobbyLib.Jsons
{
    public class Inventory
    {
        [JsonIgnore]
        public Guid UserId { get; set; } //ID From UserDB Id
        public List<IItem> PocketItems { get; set; } = [];
        public string BackSlot { get; set; }
        public IRig? RigSlot { get; set; }
        public WeaponSlot PrimarySlot { get; set; }
        public WeaponSlot SecondarySlot { get; set; }
        public string? MeleeSlot { get; set; }
        public IItem? HoldingItem { get; set; }


        public class WeaponSlot
        {
            public string Id { get; set; } //Gun Name
            public string? MagazineId { get; set; } //Loaded Magazine if exists
            public List<string> Ammos { get; set; } = new();

        }


        public override string ToString()
        {
            return $"Items: {string.Join(", ", PocketItems)}, InvItem: {BackSlot}, HoldingItem: {HoldingItem}";
        }
    }
}
