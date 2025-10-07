namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public PurchaseItem(){}
    public PurchaseItem(Device device, int quantity, Purchase purchase, string? description)
        {
            Device = device;
            DeviceId = device.Id;
            Purchase = purchase;
            PurchaseId = purchase.Id;
            Price = device.PriceForPurchase;
            Quantity = quantity;
            Description = description;
        }
        public Device Device { get; set; }

        public int DeviceId { get; set; }

        public Purchase Purchase { get; set; }
        public string? Description { get; set; }
        public int PurchaseId { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseItem item &&
                   EqualityComparer<Device>.Default.Equals(Device, item.Device) &&
                   DeviceId == item.DeviceId &&
                   EqualityComparer<Purchase>.Default.Equals(Purchase, item.Purchase) &&
                   Description == item.Description &&
                   PurchaseId == item.PurchaseId &&
                   Price == item.Price &&
                   Quantity == item.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Device, DeviceId, Purchase, Description, PurchaseId, Price, Quantity);
        }
    }

}
