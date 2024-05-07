using System.Reflection;
using System.Runtime.Serialization;

namespace LobbyLib.Jsons
{
    public static class Exts
    {
        public static string? GetEnumMemberValue<T>(this T value)
    where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }
    }
}
