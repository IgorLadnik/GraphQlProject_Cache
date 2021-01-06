using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GraphQlHelperLib
{
    public class DbProvider<T> where T : DbContext, new()
    {
        public Task<R> FetchAsync<R>(Func<T, R> func) =>
            Task.Run(() =>
            {
                using var dbContext = new T();
                return func(dbContext);
            });
        
        public MutationResponse Save(Action<T> action)
        {
            MutationResponse mutationResponse = new() { Status = "Success", Message = string.Empty };
            using var dbContext = new T();
            using var transaction = dbContext.Database.BeginTransaction();
            try
            {
                action(dbContext);
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e) 
            {
                mutationResponse.Status = "Failure";
                mutationResponse.Message = e.Message;
            }

            return mutationResponse;
        }
    }
}
