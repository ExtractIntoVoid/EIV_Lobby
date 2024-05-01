namespace LobbyLib.Jsons
{
    public class ItemRecreator
    {
        public string ItemBaseID { get; set; }
        public uint Amount { get; set; } = 1;
        public List<ItemRecreator> Contained { get; set; } = new();

        public string Slot { get; set; }
        public override string ToString()
        {
            return $"ItemBaseID: {ItemBaseID}, Amount: {Amount}, Slot: {Slot}";
        }
    }
}
