using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultArmoredRig : IArmoredRig
    {
        public decimal BlockEfficacy { get; set; }
        public decimal ArmorWeight { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IArmoredRig);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;
        public List<IItem> Items { get; set; } = [];
        public IArmorPlate? PlateSlot { get; set; } 
        public uint MaxItem { get; set; }
        public List<string> ItemsAccepted { get; set; } = [];
        public List<string> SpecificItemsAccepted { get; set; } = [];
        public List<string> ArmorPlateAccepted { get; set; } = [];

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {ItemType} {Weight} {AssetPath} | {BlockEfficacy} {ArmorWeight}";
        }
    }
}
