using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Entitites
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RestaurantSeeder(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                //if (!_dbContext.Users.Any())
                //{
                //    var users = GetUsers();

                //    foreach (var user in users)
                //    {
                //        user.PasswordHash = _passwordHasher.HashPassword(user, "pass1234");
                //    }

                //    _dbContext.Users.AddRange(users);
                //    _dbContext.SaveChanges();
                //}

                //if (!_dbContext.Restaurants.Any()) // check is any row in table
                //{
                //    var restaurants = GetRestaurants();
                //    _dbContext.Restaurants.AddRange(restaurants);
                //    _dbContext.SaveChanges();
                //}

            }
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    DateOfBirth = new DateTime(1980, 03, 01),
                    Email = "user@mail.com",
                    FirstName = "User",
                    LastName = "Userowski",
                    Nationality = "Polish",
                    Role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == "User")
                },
                new User()
                {
                    DateOfBirth = new DateTime(1980, 03, 01),
                    Email = "admin@mail.com",
                    FirstName = "Admin",
                    LastName = "Adminowski",
                    Nationality = "Polish",
                    Role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == "Admin")
                },
                new User()
                {
                    DateOfBirth = new DateTime(1980, 03, 01),
                    Email = "managerT@mail.com",
                    FirstName = "T",
                    LastName = "Trowski",
                    Nationality = "Polish",
                    Role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == "Manager")
                },
                new User()
                {
                    DateOfBirth = new DateTime(1980, 03, 01),
                    Email = "managerB@mail.com",
                    FirstName = "B",
                    LastName = "Browski",
                    Nationality = "Polish",
                    Role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == "Manager")
                },
                new User()
                {
                    DateOfBirth = new DateTime(1980, 03, 01),
                    Email = "managerC@mail.com",
                    FirstName = "C",
                    LastName = "Crowski",
                    Nationality = "Polish",
                    Role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == "Manager")
                },

            };

            return users;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "T-Restaurant",
                    //Owner = _dbContext
                    //    .Users
                    //    .FirstOrDefault(u => u.FirstName == "T"),

                    CreatedById = _dbContext
                        .Users
                        .Where(u => u.FirstName == "T")
                        .Select(u => u.Id)
                        .FirstOrDefault(),
                    Description = "T-Restaurant",
                    Category = "T-Restaurant",
                    ContactEmail = "trestaurant@mail.com",
                    HasDelivery = true,
                    //Owner = new User()
                    //{
                    //    DateOfBirth = new DateTime(1980, 03, 01),
                    //    Email = "managerT@mail.com",
                    //    FirstName = "T",
                    //    LastName = "Trowski",
                    //    Nationality = "Polish",
                    //    Role = _dbContext
                    //        .Roles
                    //        .FirstOrDefault(r => r.Name == "Manager")
                    //},
                    Address = new Address()
                    {
                        City = "T-city",
                        PostalCode = "T-code",
                        Street = "T-street"
                    },
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Some food",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food2",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food3",
                            Description = "Just food",
                            Price = 18,
                        },
                    }
                },
                new Restaurant()
                {
                    Name = "B-Restaurant",
                    //Owner = _dbContext
                    //    .Users
                    //    .FirstOrDefault(u => u.FirstName == "B"),

                    CreatedById = _dbContext
                        .Users
                        .Where(u => u.FirstName == "B")
                        .Select(u => u.Id)
                        .FirstOrDefault(),
                    Description = "T-Restaurant",
                    Category = "T-Restaurant",
                    ContactEmail = "trestaurant@mail.com",
                    HasDelivery = true,
                    //Owner = new User()
                    //{
                    //    DateOfBirth = new DateTime(1980, 03, 01),
                    //    Email = "managerB@mail.com",
                    //    FirstName = "B",
                    //    LastName = "Browski",
                    //    Nationality = "Polish",
                    //    Role = _dbContext
                    //        .Roles
                    //        .FirstOrDefault(r => r.Name == "Manager")
                    //},
                    Address = new Address()
                    {
                        City = "T-city",
                        PostalCode = "T-code",
                        Street = "T-street"
                    },
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Some food",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food2",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food3",
                            Description = "Just food",
                            Price = 18,
                        },
                    }
                },
                new Restaurant()
                {
                    Name = "C-Restaurant",
                    //Owner = _dbContext
                    //    .Users
                    //    .FirstOrDefault(u => u.FirstName == "C"),

                    CreatedById = _dbContext
                        .Users
                        .Where(u => u.FirstName == "C")
                        .Select(u => u.Id)
                        .FirstOrDefault(),
                    Description = "T-Restaurant",
                    Category = "T-Restaurant",
                    ContactEmail = "trestaurant@mail.com",
                    HasDelivery = true,
                    //Owner = new User()
                    //{
                    //    DateOfBirth = new DateTime(1980, 03, 01),
                    //    Email = "managerC@mail.com",
                    //    FirstName = "C",
                    //    LastName = "Crowski",
                    //    Nationality = "Polish",
                    //    Role = _dbContext
                    //        .Roles
                    //        .FirstOrDefault(r => r.Name == "Manager")
                    //},
                    Address = new Address()
                    {
                        City = "T-city",
                        PostalCode = "T-code",
                        Street = "T-street"
                    },
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Some food",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food2",
                            Description = "Just food",
                            Price = 18,
                        },
                        new Dish()
                        {
                            Name = "Some food3",
                            Description = "Just food",
                            Price = 18,
                        },
                    }
                },
                new Restaurant()
                {
                    Name = "Palermo Pizza",
                    Category = "Italian",
                    Description = "Italion best food in 1930!",
                    ContactEmail = "palermopizzalh@lostheaven.us",
                    HasDelivery = false,
                    Address = new Address()
                    {
                        City = "Lost Heaven",
                        PostalCode = "1930",
                        Street = "Salieri Boss"
                    },
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "True Italian Pizza",
                            Description = "Really true 1930 Italian pizza!",
                            Price = 16.8M
                        },
                        new Dish()
                        {
                            Name = "Lasagne",
                            Description = "Best lasagne",
                            Price = 14.4M,
                        }
                    }
                },
                new Restaurant()
                {
                    Name = "Turkish Kebab",
                    Category = "Kebab",
                    Description = "Best from meet",
                    ContactEmail = "kebabturkish@ol.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "Venger",
                        PostalCode = "2010",
                        Street = "Bizarre St."
                    },
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Kebab",
                            Description = "True kebab",
                            Price = 10.18M
                        },
                        new Dish()
                        {
                            Name = "Kebab lunch",
                            Description = "Kebab in meal state",
                            Price = 12.08M
                        }
                    }
                }
            };

            return restaurants;
        }
    }
}
