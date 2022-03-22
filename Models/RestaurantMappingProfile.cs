using AutoMapper;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;
using RestaurantAPI.Entitites;

namespace RestaurantAPI.Models
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<RegisterUserDto, User>();

            CreateMap<Dish, DishDTO>();
            CreateMap<CreateDishDto, Dish>();

            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));


            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));


            CreateMap<RestaurantModifyDto, Restaurant>()
                .ForMember(r => r.Name, c => c.MapFrom(dt => dt.Name))
                .ForMember(r => r.Description, c => c.MapFrom(dt => dt.Description))
                .ForMember(r => r.HasDelivery, c => c.MapFrom(dt => dt.HasDelivery));
        }
    }
}
