using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entitites;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        public AccountController(RestaurantDbContext dbContext, IMapper mapper, IAccountService accountService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Create([FromBody] RegisterUserDto dto)
        {
            _accountService.CreateUser(dto);

            return Ok();
        }
    }
}
