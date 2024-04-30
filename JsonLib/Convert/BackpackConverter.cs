using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class BackpackConverter : JsonConverter<IBackpack>
    {
        public override IBackpack? ReadJson(JsonReader reader, Type objectType, IBackpack? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultBackpack();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IBackpack? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
