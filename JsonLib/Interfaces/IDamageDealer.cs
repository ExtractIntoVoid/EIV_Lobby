namespace JsonLib.Interfaces;

public interface IDamageDealer
{
    public uint Damage { get; set; }
    public uint ArmorDamage { get; set; }
    public string DamageType { get; set; }
}
