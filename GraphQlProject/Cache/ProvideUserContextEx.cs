using System.Collections.Generic;
using GraphQL.Execution;

namespace GraphQlProject
{
    public static class ProvideUserContextEx 
    {
        private static IDictionary<string, object> GetCache(this IProvideUserContext context) => context.UserContext;

        public static object GetCache(this IProvideUserContext context, string key)
        {
            ProvideUserContextEx.GetCache(context).TryGetValue(key, out object cache);
            return cache;
        }

        public static void SetCache(this IProvideUserContext context, string key, object cache)
        {
            if (cache == null)
                return;
            
            ProvideUserContextEx.GetCache(context)[key] = cache;
        }
    }
}
