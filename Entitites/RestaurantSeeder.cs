using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Entitites
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Restaurants.Any()) // check is any row in table
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
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
