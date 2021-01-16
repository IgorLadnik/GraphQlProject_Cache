using Microsoft.EntityFrameworkCore;
using PersonModelLib.Models;

namespace JwtHelperLib.Data
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
                new User { Id = 1, /*StrId = "u_04",*/ UserName = "Regular", Password = "RegularPassword", Type = UserAuthType.Regular },
                new User { Id = 2, /*StrId = "u_05",*/ UserName = "Admin", Password = "AdminPassword", Type = UserAuthType.Admin },
                new User { Id = 3, /*StrId = "u_06",*/ UserName = "Super", Password = "SuperPassword", Type = UserAuthType.SuperUser }
            );
        }
    }
}
