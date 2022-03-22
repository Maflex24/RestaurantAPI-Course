using System.Linq;
using FluentValidation;
using RestaurantAPI.Controllers;
using RestaurantAPI.Models;

namespace RestaurantAPI.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly RestaurantDbContext _dbContext;

        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(e => e.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(e => e.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                    {
                        var emailInUse = _dbContext.Users.Any(u => u.Email == value);
                        if (emailInUse)
                        {
                            context.AddFailure("Email", "Your e-mail address is already in use");
                        }

                    }
                );
        }
    }
}
