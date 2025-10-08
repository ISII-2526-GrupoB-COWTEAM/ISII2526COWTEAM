namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        
        public Purchase()
        {
            PurchaseItems = new List<PurchaseItem>();
        }

        public Purchase(ApplicationUser applicationUser, string deliveryAddress, DateTime purchaseDate, IList<PurchaseItem> purchaseItems, PaymentMethodTypes paymentMethod)
        {
            TotalPrice = decimal.Round(purchaseItems.Sum(pi => pi.Price * pi.Quantity), 2);
            TotalQuantity = purchaseItems.Sum(pi => pi.Quantity);

            ApplicationUser = applicationUser;
            DeliveryAddress = deliveryAddress;
            PurchaseDate = purchaseDate;
            PurchaseItems = purchaseItems;
            PaymentMethod = paymentMethod;
        }

        public Purchase(int purchaseId, ApplicationUser applicationUser, string deliveryAddress,
            DateTime purchaseDate, IList<PurchaseItem> purchaseItems, PaymentMethodTypes paymentMethod) :
            this( applicationUser, deliveryAddress, purchaseDate, purchaseItems, paymentMethod)
        {
            Id = purchaseId;

        }

        public int Id { get; set; }

        [Precision(10, 2)]
        public decimal TotalPrice { get; set; }

        public int TotalQuantity { get; set; } 
        public DateTime PurchaseDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public IList<PurchaseItem> PurchaseItems { get; set; }
        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }
    }
}

