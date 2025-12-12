using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class DeviceForPurchaseDTO 
    {
        public DeviceForPurchaseDTO(int id, string name, string colour, decimal price, string model, string brand, int year)
        {
            Id = id;
            Name = name;
            Colour = colour;
            Price = price;
            Model = model;
            Brand = brand;
            Year = year;
        }
 
        public int Id
        {
            get; set;
        }      
        public string Name
        {
            get; set;
        }        
        public string Colour
        {
            get; set;
        }       
        public decimal Price
        {
            get; set;
        }     
        public string Model
        {
            get; set;
        }       
        public string Brand
        {
            get; set;
        }
        public int Year
        {
            get; set;
        }
        public bool Equals(DeviceForPurchaseDTO? other)
        {
            return other is not null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Colour == other.Colour &&
                   Price == other.Price &&
                   Model == other.Model &&
                   Brand == other.Brand &&
                   Year == other.Year;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Colour, Price, Model, Brand, Year);
        }
    }
}
