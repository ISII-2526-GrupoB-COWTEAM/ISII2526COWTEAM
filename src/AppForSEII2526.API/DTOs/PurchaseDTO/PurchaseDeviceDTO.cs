using System.Drawing;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseDeviceDTO 
    {
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
        public int DeviceId { get; set; }

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
        public decimal PurchasePrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool Equals(PurchaseDeviceDTO? obj)
        {
            return obj is PurchaseDeviceDTO dTO &&
                   DeviceId == dTO.DeviceId &&
                   Name == dTO.Name &&
                   Brand == dTO.Brand &&
                   Color == dTO.Color &&
                   Year == dTO.Year &&
                   Model == dTO.Model &&
                   PurchasePrice == dTO.PurchasePrice &&
                   Quantity == dTO.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceId, Name, Brand, Color, Year, Model, PurchasePrice, Quantity);
        }

    }
}
