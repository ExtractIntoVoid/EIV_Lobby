using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IRig : IItem
{
    [JsonIgnore]
    public List<IItem> Items { get; set; }
    [JsonIgnore]
    public IArmorPlate? PlateSlot { get; set; }
    public uint MaxItem { get; set; }
    public List<string> ItemsAccepted { get; set; }

    public List<string> SpecificItemsAccepted { get; set; }

    public List<string> ArmorPlateAccepted { get; set; }

}
