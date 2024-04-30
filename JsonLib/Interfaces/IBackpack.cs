using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IBackpack : IItem
{
    public decimal MaxItemWeight { get; set; }

    [JsonIgnore]
    public decimal CurrentWeight { get; set; }
}
