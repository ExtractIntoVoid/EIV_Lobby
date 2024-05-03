using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IMagazine : IItem
{
    public List<string> Ammunition { get; set; }
    public uint MagSize { get; set; }
    public List<string> SupportedAmmo { get; set; }
}
