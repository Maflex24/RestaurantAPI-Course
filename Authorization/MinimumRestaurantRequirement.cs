using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurants { get; }

        public MinimumRestaurantRequirement(int minimumRestaurants)
        {
            MinimumRestaurants = minimumRestaurants;
        }
    }
}
