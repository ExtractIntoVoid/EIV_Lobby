using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultBackpack : IBackpack
    {
        public decimal MaxItemWeight { get; set; }
        public decimal CurrentWeight { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IBackpack);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {SubType} {ItemType} {Weight} {AssetPath} | {MaxItemWeight} {CurrentWeight}";
        }
    }
}
