using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultMelee : IMelee
    {
        public bool CanUse { get; set; }
        public float UseTime { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IMelee);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;
        public uint Damage { get; set; }
        public uint ArmorDamage { get; set; }
        public string DamageType { get; set; } = string.Empty;

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
