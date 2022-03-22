using AutoMapper;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;
using RestaurantAPI.Entitites;

namespace RestaurantAPI.Models
{
    public class DishMappingProfile : Profile
    {
        public DishMappingProfile()
        {
            CreateMap<CreateDishDto, Dish>();
            CreateMap<Dish, DishDTO>();

            //CreateMap<CreateRestaurantDto, Restaurant>()
            //    .ForMember(r => r.Address,
            //        c => c.MapFrom(dto => new Address()
            //        { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));


        }
    }
}
