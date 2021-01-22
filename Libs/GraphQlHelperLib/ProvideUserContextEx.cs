﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using GraphQL.Execution;
using JwtAuthLib;

namespace GraphQlHelperLib
{
    public interface IGqlCache 
    {
        object Value { get; set; }
    }

    public static class ProvideUserContextEx 
    {
        private static IDictionary<string, object> GetCacheDictionary(this IProvideUserContext context) => context.UserContext;

        public static bool DoesCacheExist(this IProvideUserContext context, string key) =>
            GetCacheDictionary(context).ContainsKey(key);

        
        // Cache

        public static T GetCache<T>(this IProvideUserContext context, string key)
        {
            if (!GetCacheDictionary(context).TryGetValue(key, out object cacheObj))
                return default;

            return (T)(cacheObj as IGqlCache).Value;
        }

        public static void SetCache<T>(this IProvideUserContext context, string key, object value) where T : IGqlCache, new()
        {
            if (value == null)
                return;

            T cacheObj = new();
            cacheObj.Value = value;
            GetCacheDictionary(context)[key] = cacheObj;
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

        public static bool IsInRole(this IProvideUserContext context, params UserAuthType[] userAuthTypes)
        {
            var user = context.GetUser();
            //var claims = user?.Claims;
            if (user != null)
                foreach (var userAuthType in userAuthTypes)
                    if (user.IsInRole($"{userAuthType}"))
                        return true;

            return false;
        }

        public static void ValidateRole(this IProvideUserContext context, params UserAuthType[] userAuthTypes)
        {
            if (!context.IsInRole(userAuthTypes))
                throw new UnauthorizedAccessException();
        }
    }
}