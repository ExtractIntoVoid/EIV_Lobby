namespace EIV_DataPack
{
    public class DataPack
    {
        public List<string> FileNames = new();
        internal Dictionary<string, long> FileNameToData = new();
        internal Dictionary<string, byte[]> FileNameToMetadata = new();
    }
}
