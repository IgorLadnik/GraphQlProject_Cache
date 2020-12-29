using System.Collections.Generic;
using GraphQL.Execution;

namespace GraphQlProject
{
    public static class ProvideUserContextEx 
    {
        public const string strCache = "cache";
        public const string strCacheName = "cache-name";

        public static Dictionary<string, object> GetCacheDictionary(this IProvideUserContext context) => 
            context.UserContext.TryGetValue(strCache, out object obj)
                ? obj as Dictionary<string, object>
                : null;

        public static object GetCache(this IProvideUserContext context, string key)
        {
            var dct = ProvideUserContextEx.GetCacheDictionary(context);
            dct.TryGetValue(key, out object cache);
            return cache;
        }

        public static void SetCache(this IProvideUserContext context, string key, object cache)
        {
            if (cache == null)
                return;
            
            var dct = ProvideUserContextEx.GetCacheDictionary(context);
            if (dct == null)
                context.UserContext[strCache] = new Dictionary<string, object>();

            ProvideUserContextEx.GetCacheDictionary(context)[key] = cache;
        }
    }
}
