using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.RentalDTOs
{
    public class RentalForDetailDTO : RentalForCreateDTO
    {
        public RentalForDetailDTO(int id, string applicationUserName, string applicationUserSurname, string deliveryAddress, double totalPrice, DateTime rentalDate, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalDeviceDTO> rentalDevices, PaymentMethodTypes paymentMethod)
         : base(applicationUserName, applicationUserSurname, deliveryAddress, paymentMethod, rentalDate, rentalDateFrom, rentalDateTo, rentalDevices)
        {
            Id = id;
            RentalDate = rentalDate;
        }
        public int Id { get; set; }

       
    }
}
