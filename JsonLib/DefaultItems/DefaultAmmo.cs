using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultAmmo : IAmmo
    {
        public uint Damage { get; set; }
        public uint ArmorDamage { get; set; }
        public string DamageType { get; set; } = string.Empty;
        public float Speed { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IAmmo);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {ItemType} {Weight} {AssetPath} {Damage} {DamageType} {ArmorDamage} {Speed}";
        }
    }
}
