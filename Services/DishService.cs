using System;
using System.Collections;
using System.Collections.Generic;
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
        public ActionResult<int> CreateDish(int restaurantId, CreateDishDto dto);
        public List<DishDTO> GetAllDishes(int restaurantId);
        public DishDTO GetDish(int restaurantId, int dishId);
        public void DeleteDishes(int restaurantId);
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


        public ActionResult<int> CreateDish(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public List<DishDTO> GetAllDishes(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException($"Restaurant was not found");

            var dishes = _dbContext
                .Dishes
                .Where(d => d.RestaurantId == restaurantId)
                .ToList();

            if (!dishes.Any())
                throw new NotFoundException("Can't find any dishes");

            var dishesDto = _mapper.Map<List<DishDTO>>(dishes);

            return dishesDto;
        }

        public DishDTO GetDish(int restaurantId, int dishId)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId);

            if (dish == null)
                throw new NotFoundException("Dish not found, or is not assigned to this restaurant");

            var dishDto = _mapper.Map<DishDTO>(dish);

            return dishDto;
        }

        public void DeleteDishes(int restaurantId)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException("Restaurant not found");

            var dishes = _dbContext
                .Dishes
                .Where(d => d.RestaurantId == restaurantId);

            _dbContext.Dishes.RemoveRange(dishes);
            _dbContext.SaveChanges();
        }
    }
}
