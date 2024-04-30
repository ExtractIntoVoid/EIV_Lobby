using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultArmorPlate : IArmorPlate
    {
        public decimal BlockEfficacy { get; set; }
        public decimal ArmorWeight { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IArmorPlate);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public uint Durability { get; set; }

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
