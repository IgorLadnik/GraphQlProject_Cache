using Microsoft.EntityFrameworkCore;
using PersonModelLib.Models;

namespace JwtHelperLib.Data
{
    public class UserDbContext : DbContext
    {
        public static string ConnectionString { private get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
