using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.PurchaseDTO
{
    public class PurchaseForDetailDTO : PurchaseForCreateDTO
    {
        public PurchaseForDetailDTO(int id, string applicationUserName, string applicationUserSurname, string deliveryAddress, DateTime purchaseDate, decimal totalPrice, IList<PurchaseDeviceDTO> purchaseDevices, PaymentMethodTypes paymentMethod)
             : base(applicationUserName, applicationUserSurname, deliveryAddress,paymentMethod, purchaseDate, purchaseDevices )
        {
            Id = id;
            PurchaseDate = purchaseDate;
        }
        public int Id { get; set; }

        [JsonPropertyName("FechaCompra")]
        public DateTime PurchaseDate { get; set; }





    }
}