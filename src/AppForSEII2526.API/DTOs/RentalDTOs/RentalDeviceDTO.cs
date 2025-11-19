using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.DTOs.RentalDTO
{
    public class RentalDeviceDTO
    {
        public RentalDeviceDTO(int deviceID, string name, string brand, string color, int year, string model, double priceForRent)
        {
            DeviceID = deviceID;
            Name = name;
            Brand = brand;
            Color = color;
            Year = year;
            Model = model;
            PriceForRent = priceForRent;
        }
        [Required]
        public int DeviceID { get; set; }
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

        public override bool Equals(object? obj)
        {
            return obj is RentalDeviceDTO dTO &&
                   DeviceID == dTO.DeviceID &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Color == dTO.Color &&
                   Year == dTO.Year &&
                   Model == dTO.Model &&
                   PriceForRent == dTO.PriceForRent;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceID, Name, Brand, Color, Year, Model, PriceForRent);
        }

    }
}
