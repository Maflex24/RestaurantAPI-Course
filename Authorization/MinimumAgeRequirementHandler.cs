using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.GivenName).Value;

            if (!context.User.HasClaim(c => c.Type == "DateOfBirth"))
            {
                _logger.LogInformation($" User [{userEmail}] has no specified date of birth");
                throw new DateOfBirthNotDefined("User has not defined date of birth");
            }

            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            string log = $" User [{userEmail}] with date of birth: [{dateOfBirth:yyyy-MM-dd}] Authorization of [Minimum18YearsOld]";
            var addedYears = dateOfBirth.AddYears(requirement.MinimumAge);

            if (addedYears <= DateTime.Now)
            {
                context.Succeed(requirement);
                log += " succeed";
            }
            else
            {
                log += " failed";
            }

            _logger.LogInformation(log);
            return Task.CompletedTask;
        }
    }
}
