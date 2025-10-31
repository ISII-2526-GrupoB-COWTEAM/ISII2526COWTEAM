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

        [JsonPropertyName("DeviceId")]
        public int DeviceId { get; set; }

        [Display(Name = "PurchasePrice")]
        [JsonPropertyName("PurchasePrice")]
        public decimal PurchasePrice { get; set; }

        [JsonPropertyName("Brand")]
        public string Brand { get; set; }

        [JsonPropertyName("Model")]
        public string Model { get; set; }

        [Required]
        [JsonPropertyName("Quantity")]
        // This is defined to check that at least one device is purchased
        [Range(1, double.MaxValue, ErrorMessage = "You must provide a valid quantity")]
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PurchaseDeviceDTO model &&
                   DeviceId == model.DeviceId &&
                   PurchasePrice == model.PurchasePrice &&
                   Quantity == model.Quantity &&
                   Brand == model.Brand &&
                   Model == model.Model;
        }
    }
}
