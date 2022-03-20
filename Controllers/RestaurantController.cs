using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Entitites;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController] // automatycznie wywołuje walidację modelu ModelState.isValid..
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(RestaurantDbContext dbContext, IMapper mapper, IRestaurantService restaurantService) // gdy wstrzykujemy coś przez konstruktor, musimy dodać restaurantService
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] RestaurantModifyDto dto)
        {
            _restaurantService.UpdateRestaurant(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.CreateRestaurant(dto);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurant()
        {
            var results = _restaurantService.GetAllRestaurants();

            if (results.Any())
                return Ok(results);

            return BadRequest("Records not funded");
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetRestaurant([FromRoute] int id)
        {
            _restaurantService.GetById(id);

            return Ok();
        }

    }
}
