using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LobbyLib.Jsons
{
    public class UserInfoJson_JWT
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum DRMType
        {
            [EnumMember(Value = "unknown")]
            Unknown = -1,
            [EnumMember(Value = "nodrm")]
            DRM_FREE = 0,
            [EnumMember(Value = "steam")]
            DRM_STEAM = 1,
            [EnumMember(Value = "epic")]
            DRM_EPIC = 2,
            // more drm?
        }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public DRMType DRM { get; set; } = DRMType.Unknown;
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Version { get; set; }
        public object AdditionalData { get; set; }
        public string CreateUserId()
        {
            return $"{UserId}@{Exts.GetEnumMemberValue(DRM)}";
        }
    }
}
