using JsonLib.Interfaces;

namespace JsonLib.Items
{
    public class DefaultItem : IItem
    {
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IItem);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {ItemType} {Weight} {AssetPath}";
        }
    }
}
