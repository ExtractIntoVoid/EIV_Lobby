using EIV_JsonLib.Interfaces;
using Newtonsoft.Json;

namespace LobbyLib.Jsons
{
    public class Inventory
    {
        [JsonIgnore]
        public Guid UserId { get; set; } //ID From UserDB Id
        public List<IItem> PocketItem { get; set; } = [];
        public string BackSlotId { get; set; }
        public IRig? RigSlot { get; set; }
        public WeaponSlot? PrimarySlot { get; set; }
        public WeaponSlot? SecondarySlot { get; set; }
        public IMelee? MeleeSlot { get; set; }
        public IItem? HoldingItem { get; set; }


        public class WeaponSlot
        {
            public IGun Gun { get; set; }
            // attachment and stuff0

        }


        public override string ToString()
        {
            return $"Items: {string.Join(", ", PocketItem)}, BackSlotId: {BackSlotId}, HoldingItem: {HoldingItem}";
        }
    }
}
