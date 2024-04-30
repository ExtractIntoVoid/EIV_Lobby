namespace JsonLib_Mod.Internal
{
    public class BasicHelmet : IHelmet
    {
        public bool IsHelmet { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string SubType { get; set; } = string.Empty;
        public string ItemType { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {SubType} {ItemType} {Weight} {AssetPath} {IsHelmet}";
        }
    }
}
