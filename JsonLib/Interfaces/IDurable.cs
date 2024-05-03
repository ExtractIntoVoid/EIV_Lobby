namespace JsonLib.Interfaces;

public interface IDurable : IItem
{
    public uint Durability { get; set; }
}
