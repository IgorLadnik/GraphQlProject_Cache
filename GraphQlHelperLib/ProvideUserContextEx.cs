using System.Collections.Generic;
using System.Security.Claims;
using GraphQL.Execution;

namespace GraphQlHelperLib
{
    public static class ProvideUserContextEx 
    {
        private static IDictionary<string, object> GetCacheDictionary(this IProvideUserContext context) => context.UserContext;

        public static bool DoesCacheExist(this IProvideUserContext context, string key) =>
            GetCacheDictionary(context).ContainsKey(key);

        
        // Cache

        public static T GetCache<T>(this IProvideUserContext context, string key)
        {
            GetCacheDictionary(context).TryGetValue(key, out object cache);
            return (T)cache;
        }

        public static void SetCache(this IProvideUserContext context, string key, object cache)
        {
            if (cache == null)
                return;
            
            GetCacheDictionary(context)[key] = cache;
        }


        // User for authentication

        public static ClaimsPrincipal GetUser(this IProvideUserContext context)
        {
            return GetCacheDictionary(context).TryGetValue("_", out object user)
                        ? (ClaimsPrincipal)user
                        : null;
        }

        public static void SetUser(this IProvideUserContext context, ClaimsPrincipal user)
        {
            if (user == null)
                return;

            GetCacheDictionary(context)["_"] = user;
        }
    }
}
