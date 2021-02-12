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
        private int countInsideLock = 0;
        private int countOutsideLock = 0;

        protected Task<object> CacheDataFromRepo(Func<Task> fetchAsync, Func<object> func, ILogger logger, string sourceForDiagnostic = null) =>
            Task.Run(async () =>
            {
                object result;
                if (_lock == null)
                {
                    logger.LogTrace($"countOutsideLock = {++countOutsideLock}");
                    result = await SafeFuncAsync(fetchAsync, func, logger, sourceForDiagnostic);
                }
                else
                    using (await _lock.LockAsync())
                    {
                        logger.LogTrace($"countInsideLock = {++countInsideLock}");
                        result = await SafeFuncAsync(fetchAsync, func, logger, sourceForDiagnostic);
                        _lock = null;
                    }

                return result;
            });

        //protected Task<object> CacheDataFromRepo(Func<Task> fetchAsync, Func<object> func, ILogger logger, string sourceForDiagnostic = null) =>
        //    Task.Run(async () =>
        //    {
        //        using (await _lock.LockAsync())
        //            return await SafeFuncAsync(fetchAsync, func, logger, sourceForDiagnostic);
        //    });

        private static async Task<object> SafeFuncAsync(Func<Task> fetchAsync, Func<object> func, ILogger logger, string sourceForDiagnostic)
        {
            try
            {
                await fetchAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"ERROR in {sourceForDiagnostic}, ObjectGraphTypeCached, fetch operation.");
                return $"{e.Message}, Inner: {e.InnerException?.Message}";
            }

            try
            {
                return func();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"ERROR in {sourceForDiagnostic}, ObjectGraphTypeCached, taking data from cache.");
                return $"{e.Message}, Inner: {e.InnerException?.Message}";
            }
        }
    }
}
