using System;
using Microsoft.EntityFrameworkCore;

namespace GraphQlHelperLib
{
    public class DbProvider<T> where T : DbContext, new()
    {
        public R Fetch<R>(Func<T, R> func)
        {
            using var dbContext = new T();
            return func(dbContext);
        }
    }
}
