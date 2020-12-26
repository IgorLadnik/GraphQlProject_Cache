using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Execution;
using GraphQlProject.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQlProject
{
    public class Cache
    {
        public int CurrentNumber { get; set; }
        public object Payload { get; set; }
    }

    public static class IProvideUserContextEx 
    {
        public const string strCache = "cache";
        public const string strCacheName = "cache-name";

        //public static string GetCurrentCacheName(this IProvideUserContext context) => (string)context.UserContext[strCacheName];
        //public static void SetCurrentCacheName(this IProvideUserContext context, string cacheName) => context.UserContext[strCacheName] = cacheName;

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
    }
}
