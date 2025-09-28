namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        String CustomerUserName { get; set; }
        String CustomerSurname { get; set; }
        String DeliveryAddress { get; set; }
        int Id { get; set; }
        PaymentMethod paymentMethod { get; set; }
        DateTime PurchaseDate { get; set; }
        double TotalPrice { get; set; }
        int TotalQuantity { get; set; }

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
