using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class MagazineConverter : JsonConverter<IMagazine>
    {
        public override IMagazine? ReadJson(JsonReader reader, Type objectType, IMagazine? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultMagazine();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IMagazine? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
