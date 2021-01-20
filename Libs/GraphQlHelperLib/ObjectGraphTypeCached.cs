using System;
using System.Threading.Tasks;
using GraphQL.Types;
using AsyncLockLib;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        private readonly AsyncLock _lock = new();

        protected Task FirstCall(Func<Task> func) =>
            Task.Run(async () =>
            {
                using (await _lock.LockAsync())
                    await func();
            });
    }
}
