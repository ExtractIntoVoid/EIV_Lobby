using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultStash : IStash
    {
        public uint MaxSize { get; set; }
        public uint MaxWeight { get; set; }
        public List<string> Items { get; set; } = [];
    }
}
