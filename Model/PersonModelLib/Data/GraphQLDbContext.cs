using Microsoft.EntityFrameworkCore;
using PersonModelLib.Models;

namespace PersonModelLib.Data
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
                
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Affiliation>().HasData(
                new Affiliation { Id =  -1, Since = 2018, OrganizationId = -3, RoleId = -1, PersonId = -1 },
                new Affiliation { Id =  -2, Since = 2015, OrganizationId = -3, RoleId = -2, PersonId = -2 },
                new Affiliation { Id =  -3, Since = 2017, OrganizationId = -1, RoleId = -3, PersonId = -2 },
                new Affiliation { Id =  -4, Since = 2014, OrganizationId = -5, RoleId = -2, PersonId = -3 },
                new Affiliation { Id =  -5, Since = 2018, OrganizationId = -4, RoleId = -1, PersonId = -1 },
                new Affiliation { Id =  -6, Since = 2018, OrganizationId = -5, RoleId = -1, PersonId = -4 },
                new Affiliation { Id =  -7, Since = 2018, OrganizationId = -5, RoleId = -2, PersonId = -5 },
                new Affiliation { Id =  -8, Since = 2020, OrganizationId = -2, RoleId = -3, PersonId = -5 },
                new Affiliation { Id =  -9, Since = 2019, OrganizationId = -5, RoleId = -1, PersonId = -6 },
                new Affiliation { Id = -10, Since = 2017, OrganizationId = -4, RoleId = -1, PersonId = -7 }
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization { Id = -1, Name = "University A",           Address = "1, A St.",            ParentId = -1000 },
                new Organization { Id = -2, Name = "University B",           Address = "2, B St.",            ParentId = -1000 },
                new Organization { Id = -3, Name = "Dept. CS, University A", Address = "1, A St., 1st floor", ParentId = -1 },
                new Organization { Id = -4, Name = "Dept. ME, University A", Address = "1, A St., 2nd floor", ParentId = -1 },
                new Organization { Id = -5, Name = "Dept. EE, University B", Address = "2, B St., 1st floor", ParentId = -2 }
            );

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = -1, GivenName = "John",      Surname = "Croft",      Born = 2000, Phone = "237-006-171", Email = "jcroft@ua.ac.zz",     Address = "1, Plum St." },
                new Person { Id = -2, GivenName = "Moshe",     Surname = "Cohen",      Born = 1970, Phone = "456-543-543", Email = "mcohen@ua.ac.zz",     Address = "5, Olive Av." },
                new Person { Id = -3, GivenName = "Ann",       Surname = "Anders",     Born = 1980, Phone = "321-146-377", Email = "aanders@ub.ac.zz",    Address = "10, Apple Dr." },
                new Person { Id = -4, GivenName = "Alexander", Surname = "Petrov",     Born = 1990, Phone = "860-473-007", Email = "apetrov@ub.ac.zz",    Address = "19, Peach Rd." },
                new Person { Id = -5, GivenName = "Jose",      Surname = "Hernandez",  Born = 1967, Phone = "206-178-211", Email = "jhernandez@ub.ac.zz", Address = "35, Orange St." },
                new Person { Id = -6, GivenName = "Amit",      Surname = "Srivastava", Born = 2001, Phone = "",            Email = "srivastava@ub.ac.zz", Address = "" },
                new Person { Id = -7, GivenName = "Matteo",    Surname = "Feretti",    Born = 1999, Phone = "",            Email = "feretti@ub.ac.zz",    Address = "" }
            );

            modelBuilder.Entity<Relation>().HasData(
                new Relation { Id = -1, Since = 2020, Kind = "committee", Notes = "",                    P1Id = -2, P2Id = -5 },
                new Relation { Id = -2, Since = 2016, Kind = "superior",  Notes = "",                    P1Id = -1, P2Id = -2 },
                new Relation { Id = -3, Since = 2016, Kind = "superior",  Notes = "",                    P1Id = -3, P2Id = -5 },
                new Relation { Id = -4, Since = 2016, Kind = "superior",  Notes = "",                    P1Id = -4, P2Id = -5 },
                new Relation { Id = -5, Since = 2019, Kind = "superior",  Notes = "a",                   P1Id = -6, P2Id = -3 },
                new Relation { Id = -6, Since = 2019, Kind = "friends",   Notes = "the closest friends", P1Id = -6, P2Id = -4 },
                new Relation { Id = -7, Since = 2017, Kind = "friends",   Notes = "",                    P1Id = -7, P2Id = -1 },
                new Relation { Id = -8, Since = 2017, Kind = "superior",  Notes = "",                    P1Id = -7, P2Id = -2 }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = -1, Name = "Student",   Description = "Subject to brainwash" },
                new Role { Id = -2, Name = "Professor", Description = "Brainwasher" },
                new Role { Id = -3, Name = "Director",  Description = "Main brainwasher" }
            );
        }
    }
}
