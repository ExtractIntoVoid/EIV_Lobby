using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class MeleeConverter : JsonConverter<IMelee>
    {
        public override IMelee? ReadJson(JsonReader reader, Type objectType, IMelee? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultMelee();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IMelee? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
