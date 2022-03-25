using System;

namespace RestaurantAPI.Exceptions
{
    public class DateOfBirthNotDefined : Exception
    {
        public DateOfBirthNotDefined(string message) : base(message)
        {

        }
    }
}
