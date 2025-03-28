using System.Text.Json;

namespace aspnetcore.Commons.Extensions
{
    public static class JsonExtensions
    {
        public static T ParseJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
