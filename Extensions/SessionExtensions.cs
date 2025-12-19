using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Kolbasin_lab1.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T item)
        {
            var serializedItem = JsonSerializer.Serialize(item);
            session.SetString(key, serializedItem);
        }

        public static T? Get<T>(this ISession session, string key) where T : class
        {
            var item = session.GetString(key);
            return item == null
                ? default(T)
                : JsonSerializer.Deserialize<T>(item);
        }
    }
}

