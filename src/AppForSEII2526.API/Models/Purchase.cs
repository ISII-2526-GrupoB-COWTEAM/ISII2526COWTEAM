namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public String CustomerUserName { get; set; }
        public String CustomerSurname { get; set; }
        public String DeliveryAddress { get; set; }
        public int Id { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double TotalPrice { get; set; }
        public int TotalQuantity { get; set; }

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
    }
}
