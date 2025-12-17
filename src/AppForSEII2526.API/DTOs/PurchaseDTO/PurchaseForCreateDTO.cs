

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseForCreateDTO
    {
        public PurchaseForCreateDTO(string name, string surname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime purchaseDate,  IList<PurchaseDeviceDTO> purchaseDevices)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            Surname = surname ?? throw new ArgumentException(nameof(surname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            PurchaseDate = purchaseDate;
            PurchaseDevices = purchaseDevices ?? throw new ArgumentException(nameof(purchaseDevices));
        }

        public IList<PurchaseDeviceDTO> PurchaseDevices { get; set; }
        public DateTime PurchaseDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }


        [Required]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string Surname { get; set; }



        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public decimal TotalPrice{ get; set; }

       
    }
}
