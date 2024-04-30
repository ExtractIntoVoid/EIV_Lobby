using JsonLib.Interfaces;
using JsonLib.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace JsonLib.Convert
{
    public class ItemConverter : JsonConverter<IItem>
    {
        public override IItem? ReadJson(JsonReader reader, Type objectType, IItem? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var item = (IItem)new DefaultItem();
            var jsonObject = JObject.Load(reader);
            JsonConverter? conv = null;
            if (jsonObject.GetValue("ItemType") == null)
            {
                return item;
            }
            if (jsonObject["ItemType"] == null)
            {
                return item;
            }
            if (jsonObject["ItemType"]!.ToString() == nameof(IItem))
            {
                return item;
            }

            conv = JsonLibConverters.ModdedConverters.Where(x => x.GetJsonConverter(jsonObject["ItemType"]!.ToString()) != null).First().GetJsonConverter(jsonObject["ItemType"]!.ToString());

            if (conv != null)
            {
                if (conv.ReadJson(jsonObject.CreateReader(), objectType, existingValue, serializer) is not IItem item_check)
                    return item;
                return item_check;
            }
            return item;
        }

        public override void WriteJson(JsonWriter writer, IItem? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
