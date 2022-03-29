using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;
using RestaurantAPI.Controllers;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantRequirementHandler : AuthorizationHandler<MinimumRestaurantRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;
        private readonly RestaurantDbContext _dbContext;

        public MinimumRestaurantRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, RestaurantDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantRequirement requirement)
        {
            _logger.LogInformation("HandleRequirementAsync of MinimumRestaurantRequirementHandler was called");
            int? userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var restaurantsCreated = _dbContext
                .Restaurants
                .Count(r => r.CreatedById == userId);


            if (restaurantsCreated >= requirement.MinimumRestaurants)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
