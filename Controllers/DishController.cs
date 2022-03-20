using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/{restaurantId}/dish")]
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

            return Created($"/api/{restaurantId}/{id}", null);
        }
    }
}
