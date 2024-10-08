﻿using EIV_Common.Inventory;
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
        public ISlot<IRig>? RigSlot { get; set; }
        public GunSlot? PrimarySlot { get; set; }
        public GunSlot? SecondarySlot { get; set; }
        public ISlot<IMelee>? MeleeSlot { get; set; }
        public IItem? HoldingItem { get; set; }


        public override string ToString()
        {
            return $"Items: {string.Join(", ", PocketItem)}, BackSlotId: {BackSlotId}, HoldingItem: {HoldingItem}";
        }
    }
}
