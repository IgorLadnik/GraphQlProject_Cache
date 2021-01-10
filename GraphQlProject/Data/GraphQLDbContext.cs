using Microsoft.EntityFrameworkCore;
using GraphQlProject.Models;
using JwtHelperLib;

namespace GraphQlProject.Data
{
    public class GraphQLDbContext : DbContext
    {
        public static string ConnectionString { private get; set; }

        public GraphQLDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<Affiliation> Affiliations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Affiliation>().HasData(
                new Affiliation { Id =  1, /*StrId = "a_01",*/ Since = 2018, OrganizationId = 3, RoleId = 1, PersonId = 1 },
                new Affiliation { Id =  2, /*StrId = "a_02",*/ Since = 2015, OrganizationId = 3, RoleId = 2, PersonId = 2 },
                new Affiliation { Id =  3, /*StrId = "a_03",*/ Since = 2017, OrganizationId = 1, RoleId = 3, PersonId = 2 },
                new Affiliation { Id =  4, /*StrId = "a_04",*/ Since = 2014, OrganizationId = 5, RoleId = 2, PersonId = 3 },
                new Affiliation { Id =  5, /*StrId = "a_05",*/ Since = 2018, OrganizationId = 4, RoleId = 1, PersonId = 1 },
                new Affiliation { Id =  6, /*StrId = "a_06",*/ Since = 2018, OrganizationId = 5, RoleId = 1, PersonId = 4 },
                new Affiliation { Id =  7, /*StrId = "a_07",*/ Since = 2018, OrganizationId = 5, RoleId = 2, PersonId = 5 },
                new Affiliation { Id =  8, /*StrId = "a_08",*/ Since = 2020, OrganizationId = 2, RoleId = 3, PersonId = 5 },
                new Affiliation { Id =  9, /*StrId = "a_09",*/ Since = 2019, OrganizationId = 5, RoleId = 1, PersonId = 6 },
                new Affiliation { Id = 10, /*StrId = "a_10",*/ Since = 2017, OrganizationId = 4, RoleId = 1, PersonId = 7 }
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization { Id = 1, /*StrId = "o_a",*/  Name = "University A",           Address = "1, A St.",            ParentId = -1 },
                new Organization { Id = 2, /*StrId = "o_b",*/  Name = "University B",           Address = "2, B St.",            ParentId = -1 },
                new Organization { Id = 3, /*StrId = "o_a1",*/ Name = "Dept. CS, University A", Address = "1, A St., 1st floor", ParentId = 1 },
                new Organization { Id = 4, /*StrId = "o_a2",*/ Name = "Dept. ME, University A", Address = "1, A St., 2nd floor", ParentId = 1 },
                new Organization { Id = 5, /*StrId = "o_b1",*/ Name = "Dept. EE, University B", Address = "2, B St., 1st floor", ParentId = 2 }
            );

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, /*StrId = "p_01",*/ GivenName = "John",      Surname = "Croft",      Born = 2000, Phone = "237-006-171", Email = "jcroft@ua.ac.zz",     Address = "1, Plum St." },
                new Person { Id = 2, /*StrId = "p_02",*/ GivenName = "Moshe",     Surname = "Cohen",      Born = 1970, Phone = "456-543-543", Email = "mcohen@ua.ac.zz",     Address = "5, Olive Av." },
                new Person { Id = 3, /*StrId = "p_03",*/ GivenName = "Ann",       Surname = "Anders",     Born = 1980, Phone = "321-146-377", Email = "aanders@ub.ac.zz",    Address = "10, Apple Dr." },
                new Person { Id = 4, /*StrId = "p_04",*/ GivenName = "Alexander", Surname = "Petrov",     Born = 1990, Phone = "860-473-007", Email = "apetrov@ub.ac.zz",    Address = "19, Peach Rd." },
                new Person { Id = 5, /*StrId = "p_05",*/ GivenName = "Jose",      Surname = "Hernandez",  Born = 1967, Phone = "206-178-211", Email = "jhernandez@ub.ac.zz", Address = "35, Orange St." },
                new Person { Id = 6, /*StrId = "p_06",*/ GivenName = "Amit",      Surname = "Srivastava", Born = 2001, Phone = "",            Email = "",                    Address = "" },
                new Person { Id = 7, /*StrId = "p_07",*/ GivenName = "Matteo",    Surname = "Feretti",    Born = 1999, Phone = "",            Email = "",                    Address = "" }
            );

            modelBuilder.Entity<Relation>().HasData(
                new Relation { Id = 1, /*StrId = "r_01",*/ Since = 2020, Kind = "committee", Notes = "",                    P1Id = 2, P2Id = 5 },
                new Relation { Id = 2, /*StrId = "r_02",*/ Since = 2016, Kind = "superior",  Notes = "",                    P1Id = 1, P2Id = 2 },
                new Relation { Id = 3, /*StrId = "r_03",*/ Since = 2016, Kind = "superior",  Notes = "",                    P1Id = 3, P2Id = 5 },
                new Relation { Id = 4, /*StrId = "r_04",*/ Since = 2016, Kind = "superior",  Notes = "",                    P1Id = 4, P2Id = 5 },
                new Relation { Id = 5, /*StrId = "r_05",*/ Since = 2019, Kind = "superior",  Notes = "a",                   P1Id = 6, P2Id = 3 },
                new Relation { Id = 6, /*StrId = "r_06",*/ Since = 2019, Kind = "friends",   Notes = "the closest friends", P1Id = 6, P2Id = 4 },
                new Relation { Id = 7, /*StrId = "r_07",*/ Since = 2017, Kind = "friends",   Notes = "",                    P1Id = 7, P2Id = 1 },
                new Relation { Id = 8, /*StrId = "r_08",*/ Since = 2017, Kind = "superior",  Notes = "",                    P1Id = 7, P2Id = 2 }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, /*StrId = "l_st",*/ Name = "Student",   Description = "Subject to brainwash" },
                new Role { Id = 2, /*StrId = "l_pr",*/ Name = "Professor", Description = "Brainwasher" },
                new Role { Id = 3, /*StrId = "l_dr",*/ Name = "Director",  Description = "Main brainwasher" }
            );

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
