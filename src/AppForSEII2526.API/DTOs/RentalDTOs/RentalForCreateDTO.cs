using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.RentalDTOs
{
    public class RentalForCreateDTO
    {
        public RentalForCreateDTO(string name, string surname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalDeviceDTO> rentalDevices)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            Surname = surname ?? throw new ArgumentException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            RentalDateFrom = rentalDateFrom;
            RentalDateTo = rentalDateTo;
            RentalDevices = rentalDevices ?? throw new ArgumentException(nameof(rentalDevices));



        }
        
        public RentalForCreateDTO()
        {
            RentalDevices = new List<RentalDeviceDTO>();

        }

        public IList<RentalDeviceDTO> RentalDevices { get; set; }
        public DateTime RentalDateFrom { get; set; }
        public DateTime RentalDateTo { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress {get; set; }

        [EmailAddress]
        [Required]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string Surname { get; set; }

       

        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        private int NumberOfDays
        {
            get
            {
                return (RentalDateTo - RentalDateFrom).Days;
            }
        }
        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public double TotalPrice
        {
            get
            {
                return RentalDevices .Sum(ri => ri.PriceForRent * NumberOfDays);
            }
        }
    }
}
