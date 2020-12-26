using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQlProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQlProject.Data
{
    public class GraphQLDbContext : DbContext
    {
        public GraphQLDbContext(DbContextOptions<GraphQLDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>().HasData(
                new Menu
                {
                    Id = 1,
                    Name = "Cofee",
                    ImageUrl = "cofee.png"
                },
                new Menu
                {
                    Id = 2,
                    Name = "Hot Drinks",
                    ImageUrl = "hotdrinks.png"
                },
                new Menu
                {
                    Id = 3,
                    Name = "Cold Drinks",
                    ImageUrl = "colddrinks.png"
                });

            modelBuilder.Entity<SubMenu>().HasData(
                new SubMenu
                {
                    Id = 1,
                    Name = "Black Coffee",
                    Description = "Italian style coffee with and without sugar",
                    ImageUrl = "blackcoffee.png",
                    Price = 5,
                    MenuId = 1
                },
                new SubMenu
                {
                    Id = 2,
                    Name = "Espresso",
                    Description = "Espresso is made with coffee beans and steamed milk",
                    ImageUrl = "espresso.png",
                    Price = 10,
                    MenuId = 1
                },
                new SubMenu
                {
                    Id = 3,
                    Name = "Latte",
                    Description = "Coffee in Latte Style",
                    ImageUrl = "latte.png",
                    Price = 7,
                    MenuId = 1
                },
                new SubMenu
                {
                    Id = 4,
                    Name = "Hot Chocolate",
                    Description = "Hot Chocolate with or without sugar",
                    ImageUrl = "hotchocolate.png",
                    Price = 21,
                    MenuId = 2
                },
                new SubMenu
                {
                    Id = 5,
                    Name = "Oranje Juce",
                    Description = "Orange Juice with or without ice",
                    ImageUrl = "",
                    Price = 10,
                    MenuId = 3
                },
                new SubMenu
                {
                    Id = 6,
                    Name = "Apple Juce",
                    Description = "Apple Juice with or without sugar",
                    ImageUrl = "",
                    Price = 13,
                    MenuId = 3
                },
                new SubMenu
                {
                    Id = 7,
                    Name = "Cacao",
                    Description = "Cacao Drink",
                    ImageUrl = "cacao.png",
                    Price = 18,
                    MenuId = 2
                });

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    Name = "John",
                    Phone ="12345",
                    TotalPeople = 10,
                    Email = "john@aa.com",
                    Date = "2021-01-17",
                    Time = "11:00"
                },
                new Reservation
                {
                    Id = 2,
                    Name = "Vasya",
                    Phone = "54321",
                    TotalPeople = 20,
                    Email = "vasya@aa.com",
                    Date = "2021-01-18",
                    Time = "20:00"
                });
        }
    }
}
