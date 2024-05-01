using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultGun : IGun
    {
        public List<string> MagazineSupport { get; set; } = [];
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IGun);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;
        public IMagazine? Magazine { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {ItemType} {Weight} {AssetPath} | {string.Join(", ", MagazineSupport)}";
        }
    }
}
