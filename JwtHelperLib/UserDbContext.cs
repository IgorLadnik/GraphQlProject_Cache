using JwtHelperLib;
using Microsoft.EntityFrameworkCore;
using PersonModelLib.Models;

namespace JwtHelperLib.Data
{
    public class UserDbContext : DbContext
    {
        public static string ConnectionString { private get; set; }

        public UserDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, /*StrId = "u_01",*/ UserName = "Rachel",  Password = "rrr",             Type = UserAuthType.SuperUser },
                new User { Id = 2, /*StrId = "u_02",*/ UserName = "Sandeep", Password = "sss",             Type = UserAuthType.Admin },
                new User { Id = 3, /*StrId = "u_03",*/ UserName = "Nick",    Password = "nnn",             Type = UserAuthType.Regular },
                new User { Id = 4, /*StrId = "u_04",*/ UserName = "Regular", Password = "RegularPassword", Type = UserAuthType.Regular },
                new User { Id = 5, /*StrId = "u_05",*/ UserName = "Admin",   Password = "AdminPassword",   Type = UserAuthType.Admin },
                new User { Id = 6, /*StrId = "u_06",*/ UserName = "Super",   Password = "SuperPassword",   Type = UserAuthType.SuperUser }
            );
        }
    }
}
