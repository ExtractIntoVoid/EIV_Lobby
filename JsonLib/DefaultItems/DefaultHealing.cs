using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultHealing : IHealing
    {
        public float HealAmount { get; set; }
        public List<string> SideEffect { get; set; } = [];
        public bool CanUse { get; set; }
        public float UseTime { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IHealing);
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
