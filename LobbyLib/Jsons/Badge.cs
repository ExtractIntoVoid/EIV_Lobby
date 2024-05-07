namespace LobbyLib.Jsons
{
    public class Badge : ICloneable
    {
        public string PlayerId { get; set; }
        public string Title { get; set; }
        public string ColorHex { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"PlayerId: {PlayerId}, Title: {Title}, ColorHex: {ColorHex}";
        }
    }
}
