using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Types;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        //private AutoResetEvent _ev = new(true);
        private object _locker = new();

        protected void FirstCall(Action action)
        {
            //_ev.WaitOne();
            lock (_locker)
                action();
            //_ev.Set();
        }
    }
}
