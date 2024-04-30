using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib_Mod.Internal
{
    public class HelmetConverter : JsonConverter<IHelmet>
    {
        public override IHelmet? ReadJson(JsonReader reader, Type objectType, IHelmet? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new BasicHelmet();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IHelmet? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
