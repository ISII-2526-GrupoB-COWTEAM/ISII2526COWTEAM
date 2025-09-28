namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(Name), IsUnique = true)]
    public class Device
    {
        public Device()
        {
        }

        public Device(string brand, string color, string description, int id, string name,
                      decimal priceForPurchase, double priceForRent,
                      string quality, Model model, int quantityForPurchase, int quantityForRent, int year)
        {
            Brand = brand;
            Color = color;
            Description = description;
            Id = id;
            Name = name;
            PriceForPurchase = priceForPurchase;
            PriceForRent = priceForRent;
            Quality = quality;
            Model = model;
            QuantityForPurchase = quantityForPurchase;
            QuantityForRent = quantityForRent;
            Year = year;
        }

        [Required]
        [StringLength(30, ErrorMessage = "Brand name cannot be longer than 30 characters.")]
        public string Brand { get; set; }

        [StringLength(20, ErrorMessage = "Color name cannot be longer than 20 characters.")]
        public string Color { get; set; }

        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; }

        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Device name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "Minimum price for purchase is 0.5 ")]
        [Display(Name = "Price For Purchase")]
        [Precision(10, 2)]
        public decimal PriceForPurchase { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.5, 100, ErrorMessage = "Minimum rent price is 0.5 and maximum is 100")]
        [Display(Name = "Price For Rent")]
        public double PriceForRent { get; set; }

        [StringLength(30, ErrorMessage = "Quality description cannot be longer than 30 characters.")]
        public string Quality { get; set; }

        [Display(Name = "Quantity For Purchase")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity for purchase is 1")]
        public int QuantityForPurchase { get; set; }

        [Display(Name = "Quantity For Rent")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity for rent is 1")]
        public int QuantityForRent { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        // Relación con Model
        public Model Model { get; set; }

        // Relación con ReviewItems (un Device puede tener varios ReviewItems)
        public IList<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();
    }
}
