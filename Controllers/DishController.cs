using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RestaurantAPI.Entitites;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDishService _dishService;

        public DishController(RestaurantDbContext dbContext, IMapper mapper, IDishService dishService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _dishService = dishService;
        }


        [HttpPost]
        public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto createDishDto)
        {
            var id = _dishService.CreateDish(restaurantId, createDishDto);

            return Created($"/api/restaurant/{restaurantId}/{id}", null);
        }

        [HttpGet]
        public ActionResult GetAll([FromRoute] int restaurantId)
        {
            var results = _dishService.GetAllDishes(restaurantId);

            return Ok(results);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDTO> GetDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dish = _dishService.GetDish(restaurantId, dishId);

            return Ok(dish);
        }

        [HttpDelete]
        public ActionResult DeleteAllDishesFromRestaurant([FromRoute] int restaurantId)
        {
            _dishService.DeleteDishes(restaurantId);

            return NoContent();
        }

        [HttpDelete("{dishId}")]
        public ActionResult DeleteDishFromRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            _dishService.DeleteDish(restaurantId, dishId);

            return NoContent();
        }

    }
}
