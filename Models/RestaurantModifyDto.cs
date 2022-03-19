using System.ComponentModel.DataAnnotations;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models
{
    public class RestaurantModifyDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
    }
}
