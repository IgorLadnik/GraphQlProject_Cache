using System;
using System.Threading.Tasks;
using GraphQL.Types;
using AsyncLockLib;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        public Exception Ex { get; private set; }
        private AsyncLock _lock = new();

        protected Task<bool> CacheDataFromRepo(Func<Task> func) =>
            Task.Run<bool>(async () =>
            {
                Ex = null;
                bool isOk;
                if (_lock == null)
                    isOk = await WrapperFuncAsync(func);
                else
                    using (await _lock.LockAsync()) 
                    {
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
    }
}
