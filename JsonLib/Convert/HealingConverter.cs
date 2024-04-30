using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class HealingConverter : JsonConverter<IHealing>
    {
        public override IHealing? ReadJson(JsonReader reader, Type objectType, IHealing? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultHealing();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IHealing? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
