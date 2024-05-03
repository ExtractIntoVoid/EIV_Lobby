namespace JsonLib.Interfaces;

public interface IItem : ICloneable
{
    public string BaseID { get; set; }
    public string ItemType { get; set; }
    public decimal Weight { get; set; }
    public string AssetPath { get; set; }
    public List<string> Tags { get; set; }
}
