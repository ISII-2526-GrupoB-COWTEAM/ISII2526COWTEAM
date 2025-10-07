namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public Purchase()
        {
            PurchaseItems = new List<PurchaseItem>();
        }

        public int Id { get; set; }

        
        public double TotalPrice { get; set; }


        public DateTime PurchaseDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        public string CustomerUserName { get; set; }

        public string CustomerUserSurname { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }


        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Purchase purchase &&
                   Id == purchase.Id &&
                   TotalPrice == purchase.TotalPrice &&
                   PurchaseDate == purchase.PurchaseDate &&
                   DeliveryAddress == purchase.DeliveryAddress &&
                   CustomerUserName == purchase.CustomerUserName &&
                   CustomerUserSurname == purchase.CustomerUserSurname &&
                   EqualityComparer<IList<PurchaseItem>>.Default.Equals(PurchaseItems, purchase.PurchaseItems) &&
                   PaymentMethod == purchase.PaymentMethod;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, TotalPrice, PurchaseDate, DeliveryAddress, CustomerUserName, CustomerUserSurname, PurchaseItems, PaymentMethod);
        }

    }
}

