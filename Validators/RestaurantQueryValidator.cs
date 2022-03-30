using System.Linq;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSizes = new int[] { 5, 10, 15 };

        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"PageSize must be in [{string.Join(", ", allowedPageSizes)}]");
                    }
                });

            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);


        }
    }
}
