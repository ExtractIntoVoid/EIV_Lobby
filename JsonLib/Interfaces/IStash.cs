namespace JsonLib.Interfaces;

public interface IStash
{
    public uint MaxSize { get; set; }
    public uint MaxWeight { get; set; }
    public List<string> Items { get; set; }
}
