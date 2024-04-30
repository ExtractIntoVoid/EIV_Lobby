using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IUsable : IItem
{
    [JsonIgnore]
    bool CanUse { get; set; }
    public float UseTime { get; set; }
}
