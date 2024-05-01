using Newtonsoft.Json;

namespace JsonLib.Interfaces;

public interface IRig : IItem
{
    public List<string> ItemIds { get; set; }
    public string? PlateSlotId { get; set; }
    public uint MaxItem { get; set; }
    public List<string> ItemsAccepted { get; set; }

    public List<string> SpecificItemsAccepted { get; set; }

    public List<string> ArmorPlateAccepted { get; set; }

}
