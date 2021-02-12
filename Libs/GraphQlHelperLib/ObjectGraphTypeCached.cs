﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GraphQL.Types;
using AsyncLockLib;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        protected Exception Ex { get; private set; }
        private AsyncLock _lock = new();
        private static int countInsideLock = 0;
        private static int countOutsideLock = 0;

        protected Task<bool> CacheDataFromRepo(Func<Task> func, ILogger logger) =>
            Task.Run<bool>(async () =>
            {
                Ex = null;
                bool isOk;
                if (_lock == null)
                {
                    logger.LogTrace($"countOutsideLock = {++countOutsideLock}");
                    isOk = await WrapperFuncAsync(func);
                }
                else
                    using (await _lock.LockAsync())
                    {
                        logger.LogTrace($"countInsideLock = {++countInsideLock}");
                        isOk = await WrapperFuncAsync(func);
                        _lock = null;
                    }

                return isOk;
            });

        private async Task<bool> WrapperFuncAsync(Func<Task> func)
        {
            try
            {
                await func();
                return true;
            }
            catch (Exception e)
            {
                Ex = e;
                return false;
            }
        }

        protected string ErrorMessage => $"{Ex.Message}, Inner: {Ex.InnerException.Message}";
    }
}
