namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class DeviceForPurchaseDTO
    {
        public DeviceForPurchaseDTO(int id, string name, string colour, decimal price, string model, string brand)
        {
            Id = id;
            Name = name;
            Colour = colour;
            Price = price;
            Model = model;
            Brand = brand;
        }

        [JsonPropertyName("Id")]
        public int Id
        {
            get; set;

        }

        [Required]
        [JsonPropertyName("Name")]
        public string Name
        {
            get; set;
        }
        [Required]
        [JsonPropertyName("Colour")]
        public string Colour
        {
            get; set;
        }
        [Required]
        [JsonPropertyName("Price")]
        public decimal Price
        {
            get; set;
        }
        [Required]
        [JsonPropertyName("Model")]
        public string Model
        {
            get; set;
        }
        [Required]
        [JsonPropertyName("Brand")]
        public string Brand
        {
            get; set;
        }
    }
}
