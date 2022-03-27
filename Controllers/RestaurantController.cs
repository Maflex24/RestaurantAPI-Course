using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
            _restaurantService.UpdateRestaurant(id, dto, User);

            return Ok();
        }

        //[Authorize(Policy = "Minimum18YearsOld")]
        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id, User);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _restaurantService.CreateRestaurant(dto, userId);

            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        //[Authorize(Policy = "MinimumManagerAccess")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllRestaurant()
        {
            var results = _restaurantService.GetAllRestaurants();

            if (results.Any())
                return Ok(results);

            return BadRequest("Records not funded");
        }

        [HttpGet("{id}")]
        [AllowAnonymous] // mimo atrybutu autoryzacji, to zapytanie może być użyte bez logowania
        public ActionResult<RestaurantDto> GetRestaurant([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }

    }
}
