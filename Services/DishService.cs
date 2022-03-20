using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;
using RestaurantAPI.Entitites;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{

    public interface IDishService
    {
        public ActionResult<int> CreateDish(int RestaurantId, CreateDishDto dto);
    }
    public class DishService : IDishService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }


        public ActionResult<int> CreateDish(int RestaurantId, CreateDishDto dto)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == RestaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = RestaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }
    }
}
