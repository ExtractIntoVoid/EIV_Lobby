using JsonLib.Interfaces;
using Newtonsoft.Json;

namespace JsonLib.Convert
{
    public static class ConvertHelper
    {
        public static IItem? ConvertFromString(this string json, List<JsonConverter>? converters = null)
        {
            converters ??= [];
            var settings = GetSerializerSettings();
            foreach (var item in converters)
            {
                settings.Converters.Add(item);
            }
            return JsonConvert.DeserializeObject<IItem>(json, settings);
        }

        public static JsonSerializerSettings GetSerializerSettings()
        {
            JsonSerializerSettings jsonSerializerSettings =  new()
            {
                Converters =
                {
                    new ItemConverter(),
                },
            };

            foreach (var item in JsonLibConverters.ModdedConverters)
            {
                if (item == null)
                    continue;
                var converters = item.GetJsonConverters();
                foreach (var conv in converters)
                {
                    jsonSerializerSettings.Converters.Add(conv);
                }           
            }
            return jsonSerializerSettings;
        }
    }
}
