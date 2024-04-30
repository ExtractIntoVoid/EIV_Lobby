namespace JsonLib.Interfaces;

public interface IArmorPlate : IItem, IDurable
{
    public string Material { get; set; }
}
