using System.Drawing;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDeviceDTO
    {
        public PurchaseDeviceDTO(int deviceID, decimal purchasePrice, string brand, string model, int quantity)
        {
            DeviceId = deviceID;
            PurchasePrice = purchasePrice;
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Quantity = quantity;
        }
        public PurchaseDeviceDTO(
        int deviceID,
        string name,
        string brand,
        string color,
        int year,
        string model,
        decimal purchasePrice,
        int quantity)
        {
            DeviceId = deviceID;
            Name = name;
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Color = color;
            Year = year;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            PurchasePrice = purchasePrice;
            Quantity = quantity;
        }

        [Required]
        [JsonPropertyName("deviceID")]
        public int DeviceId { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [Required]
        [JsonPropertyName("color")]
        public string Color { get; set; }

        [Required]
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [Required]
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [Required]
        [JsonPropertyName("purchasePrice")]
        public decimal PurchasePrice { get; set; }

        [Required]
        [JsonPropertyName("Quantity")]
        // This is defined to check that at least one device is purchased
        [Range(1, double.MaxValue, ErrorMessage = "You must provide a valid quantity")]
        public int Quantity { get; set; }

        
    }
}
