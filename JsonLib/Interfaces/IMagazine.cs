using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IMagazine : IItem
{
    [JsonIgnore]
    public List<IAmmo> Ammunition { get; set; }
    public uint MagSize { get; set; }
    public List<string> AmmoSupport { get; set; }
}
