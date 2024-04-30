using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class AmmoConverter : JsonConverter<IAmmo>
    {
        public override IAmmo? ReadJson(JsonReader reader, Type objectType, IAmmo? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultAmmo();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IAmmo? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
