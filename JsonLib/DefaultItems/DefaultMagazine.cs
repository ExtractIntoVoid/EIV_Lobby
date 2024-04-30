using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultMagazine : IMagazine
    {
        public List<IAmmo> Ammunition { get; set; } = [];
        public uint AmmoCount { get; set; }
        public uint MagSize { get; set; }
        public List<string> AmmoSupport { get; set; } = [];
        public string BaseID { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IMagazine);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {SubType} {ItemType} {Weight} {AssetPath}";
        }
    }
}
