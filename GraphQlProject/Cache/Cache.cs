using System.Collections.Generic;
using GraphQL.Execution;

namespace GraphQlProject
{
    public class Cache
    {
        public object Payload { get; set; }
        public bool IsFilled { get => Payload != null; }
    }

    public static class IProvideUserContextEx 
    {
        public const string strCache = "cache";
        public const string strCacheName = "cache-name";

        public static Dictionary<string, Cache> GetCacheDictionary(this IProvideUserContext context) => 
            context.UserContext.TryGetValue(strCache, out object obj)
                ? obj as Dictionary<string, Cache>
                : null;

        public static Cache GetCache(this IProvideUserContext context, string key)
        {
            var dct = IProvideUserContextEx.GetCacheDictionary(context);
            dct.TryGetValue(key, out Cache cache);
            return cache;
        }

        public static void SetCache(this IProvideUserContext context, string key, Cache cache)
        {
            if (cache == null)
                return;
            
            var dct = IProvideUserContextEx.GetCacheDictionary(context);
            if (dct == null)
                context.UserContext[strCache] = new Dictionary<string, Cache>();

            IProvideUserContextEx.GetCacheDictionary(context)[key] = cache;
        }

        public static bool IsFilled(this Cache cache) => cache != null && cache.IsFilled;
    }
}
