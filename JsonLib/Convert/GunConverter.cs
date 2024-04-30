using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class GunConverter : JsonConverter<IGun>
    {
        public override IGun? ReadJson(JsonReader reader, Type objectType, IGun? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultGun();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IGun? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
