using GraphQlProject.Models;
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
}
