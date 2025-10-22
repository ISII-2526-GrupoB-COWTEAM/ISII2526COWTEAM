using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class DeviceForRentalDTO
    {
        public DeviceForRentalDTO(int id, string name, string brand, string color, int year, string model, double priceForRent)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Color = color;
            Year = year;
            Model = model;
            PriceForRent = priceForRent;
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public double PriceForRent { get; set; }


    }
}
