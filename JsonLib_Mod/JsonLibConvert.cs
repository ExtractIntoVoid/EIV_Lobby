using JsonLib.Modding;
using JsonLib_Mod.Internal;
using Newtonsoft.Json;

namespace JsonLib_Mod
{
    public class JsonLibConvert : IJsonLibConverter
    {
        public JsonConverter? GetJsonConverter(string ItemType)
        {
            return ItemType switch
            {
                "IHelmet" => new HelmetConverter(),
                _ => null,
            };
        }

        public List<JsonConverter> GetJsonConverters()
        {
            return
            [
                new HelmetConverter()

            ];
        }
    }
}
