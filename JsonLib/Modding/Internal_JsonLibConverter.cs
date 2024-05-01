using JsonLib.Convert;
using Newtonsoft.Json;

namespace JsonLib.Modding
{
    internal class Internal_JsonLibConverter : IJsonLibConverter
    {
        public JsonConverter? GetJsonConverter(string ItemType)
        {
            return ItemType switch
            {
                "IAmmo" => new AmmoConverter(),
                "IArmor" => new ArmorConverter(),
                "IBackpack" => new BackpackConverter(),
                "IGun" => new GunConverter(),
                "IHealing" => new HealingConverter(),
                "IMagazine" => new MagazineConverter(),
                "IMelee" => new MeleeConverter(),
                "IThrowable" => new ThrowableConverter(),
                "IArmoredRig" => new ArmoredRigConverter(),
                "IArmorPlate" => new ArmorPlateConverter(),
                "IRig" => new RigConverter(),
                _ => null,
            };
        }

        public List<JsonConverter> GetJsonConverters()
        {
            return
            [
                new AmmoConverter(),
                new ArmorConverter(),
                new BackpackConverter(),
                new GunConverter(),
                new HealingConverter(),
                new MagazineConverter(),
                new MeleeConverter(),
                new ThrowableConverter(),
                new StashConverter(),
                new ArmoredRigConverter(),
                new ArmorPlateConverter(),
                new RigConverter(),
            ];
        }
    }
}
