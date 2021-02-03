using Microsoft.EntityFrameworkCore;
using JwtLoginLib.Models;
using AuthRolesLib;

namespace JwtLoginLib.Data
{
    public class UserDbContext : DbContext
    {
        public static string ConnectionString { private get; set; }

        public DbSet<User> Users { get; set; }

        public UserDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "Regular", Password = "RegularPassword", Role = UserAuthRole.Regular },
                new User { Id = 2, UserName = "Admin",   Password = "AdminPassword",   Role = UserAuthRole.Admin },
                new User { Id = 3, UserName = "Super",   Password = "SuperPassword",   Role = UserAuthRole.SuperUser }
            );
        }
    }
}
