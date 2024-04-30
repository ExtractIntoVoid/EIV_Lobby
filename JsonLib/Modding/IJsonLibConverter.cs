using Newtonsoft.Json;

namespace JsonLib.Modding
{
    public interface IJsonLibConverter
    {
        public List<JsonConverter> GetJsonConverters();

        public JsonConverter? GetJsonConverter(string ItemType);
    }
}
