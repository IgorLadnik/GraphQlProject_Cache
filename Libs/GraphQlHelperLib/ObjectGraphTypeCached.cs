using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using GraphQL.Types;
using AsyncLockLib;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        private AsyncLock _lock = new();
        private static int countInsideLock = 0;
        private static int countOutsideLock = 0;

        protected Task<object> CacheDataFromRepo(Func<Task> fetchAsync, Func<object> func, ILogger logger) =>
            Task.Run(async () =>
            {
                object result;
                if (_lock == null)
                {
                    logger.LogTrace($"countOutsideLock = {++countOutsideLock}");
                    result = await WrapperFuncAsync(fetchAsync, func, logger);
                }
                else
                    using (await _lock.LockAsync())
                    {
                        logger.LogTrace($"countInsideLock = {++countInsideLock}");
                        result = await WrapperFuncAsync(fetchAsync, func, logger);
                        _lock = null;
                    }

                return result;
            });

        private static async Task<object> WrapperFuncAsync(Func<Task> fetchAsync, Func<object> func, ILogger logger)
        {
            try
            {
                await fetchAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in ObjectGraphTypeCached, fetch operation.");
                return $"{e.Message}, Inner: {e.InnerException?.Message}";
            }

            try
            {
                return func();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error in ObjectGraphTypeCached, taking data from cache.");
                return $"{e.Message}, Inner: {e.InnerException?.Message}";
            }
        }
    }
}
