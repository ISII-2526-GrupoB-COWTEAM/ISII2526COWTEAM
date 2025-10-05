namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId), nameof(PurchaseId))]
    public class PurchaseItem
    {
        public PurchaseItem(){}
    public PurchaseItem(Device device, int quantity, Purchase purchase)
        {
            Device = device;
            DeviceId = device.Id;
            Purchase = purchase;
            PurchaseId = purchase.Id;
            Price = device.PriceForPurchase;
            Quantity = quantity;
        }
        public Device Device { get; set; }

        public int DeviceId { get; set; }

        public Purchase Purchase { get; set; }

        public int PurchaseId { get; set; }

        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must provide a quantity higher than 1")]
        public int Quantity { get; set; }
    }

}
