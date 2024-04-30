namespace JsonLib.Interfaces;

public interface IAmmo : IItem
{
    public uint Damage { get; set; }
    public uint ArmorDamage { get; set; }
    public string DamageType { get; set; }
    public float Speed { get; set; }
}
