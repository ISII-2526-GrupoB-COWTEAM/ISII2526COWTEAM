namespace AppForSEII2526.API.Models
{
    public class Rental
    {
        public Rental()
        { 
        }
        public Rental(int id, string deliveryAddress, string name, PaymentMethodTypes paymentMethod, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, string surname, IList<RentDevice> RentDevices) 
        {
            TotalPrice = RentDevices.Sum(ri => ri.Price * (rentalDateTo - rentalDateFrom).Days);

            
            RentalDate = rentalDate;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            RentDevices = RentDevices;
            Name = name;
            Surname = surname;
            DeliveryAddress = deliveryAddress;

        }


        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime RentalDateFrom { get; set; }
        public DateTime RentalDateTo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        public IList<RentDevice> RentDevices { get; set; } 
    }
    public enum PaymentMethodTypes
    {
        CreditCard,
        PayPal,
        Cash
    }
}
