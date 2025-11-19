using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.API.DTOs.DeviceDTO
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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        
        public string Model { get; set; }
        public double PriceForRent { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DeviceForRentalDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Color == dTO.Color &&
                   Year == dTO.Year &&
                   Model == dTO.Model &&
                   PriceForRent == dTO.PriceForRent;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Brand, Color, Year, Model, PriceForRent);
        }
    }
    
}
