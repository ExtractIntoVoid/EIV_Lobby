using JsonLib.Interfaces;
using JsonLib.DefaultItems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLib.Convert
{
    public class ArmorPlateConverter : JsonConverter<IArmorPlate>
    {
        public override IArmorPlate? ReadJson(JsonReader reader, Type objectType, IArmorPlate? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = new DefaultArmorPlate();
            var jsonObject = JObject.Load(reader);
            serializer.Populate(jsonObject.CreateReader(), item);
            return item;
        }

        public override void WriteJson(JsonWriter writer, IArmorPlate? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
