namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public String CustomerUserName { get; set; }
        public String CustomerSurname { get; set; }
        
        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public String DeliveryAddress { get; set; }
        [Key]
        public int Id { get; set; }

        [Display(Name = "Método de Pago")]
        [Required()]
        public PaymentMethod paymentMethod { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

        public override bool Equals(object? obj)
        {
            return obj is Purchase purchase &&
                   CustomerUserName == purchase.CustomerUserName &&
                   CustomerSurname == purchase.CustomerSurname &&
                   DeliveryAddress == purchase.DeliveryAddress &&
                   Id == purchase.Id &&
                   EqualityComparer<PaymentMethod>.Default.Equals(paymentMethod, purchase.paymentMethod) &&
                   PurchaseDate == purchase.PurchaseDate &&
                   TotalPrice == purchase.TotalPrice &&
                   TotalQuantity == purchase.TotalQuantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerUserName, CustomerSurname, DeliveryAddress, Id, paymentMethod, PurchaseDate, TotalPrice, TotalQuantity);
        }

        public Purchase()
        {
            PurchaseItems = new List<PurchaseItem>();
        }
    }
}
