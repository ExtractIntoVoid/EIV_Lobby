namespace LobbyLib.Jsons
{
    public class ItemRecreator
    {
        public string ItemBaseID { get; set; }
        public uint Amount { get; set; } = 1;
        public List<ItemRecreator> Contained { get; set; } = new();
        public string Slot { get; set; }

        //  We set 0 always so that means not damaged any precent, only applied if it's using IDurable
        public uint Damaged { get; set; } = 0;
        public override string ToString()
        {
            return $"ItemBaseID: {ItemBaseID}, Amount: {Amount}, Slot: {Slot}, Damaged: {Damaged}";
        }
    }
}
