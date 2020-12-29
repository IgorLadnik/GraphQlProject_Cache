using System;
using Microsoft.EntityFrameworkCore;

namespace GraphQlProject.Data
{
    public class DbContextFactory
    {
        private string _connectionString;

        public DbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public GraphQLDbContext Create()
        {
            var options = new DbContextOptionsBuilder<GraphQLDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            return new GraphQLDbContext(options);
        }
    }

    public static class DbContextFactoryEx 
    {
        public static T FetchFromDb<T>(this DbContextFactory factory, Func<GraphQLDbContext, T> func) 
        {
            using (var dbContext = factory.Create())
                return func(dbContext);
        }
    }
}
