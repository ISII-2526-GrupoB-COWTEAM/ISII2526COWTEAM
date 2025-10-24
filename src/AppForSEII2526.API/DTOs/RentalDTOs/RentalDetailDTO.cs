using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.RentalDTOs
{
    public class RentalDetailDTO : RentalForCreateDTO
    {
        public RentalDetailDTO(int id, string name, string surname, string deliveryAddress, PaymentMethodTypes paymentMethod, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalDeviceDTO> rentalDevices) 
            : base( name, surname, deliveryAddress, paymentMethod, rentalDateFrom, rentalDateTo, rentalDevices)
        {
            Id = id;
            RentalDate = rentalDateFrom;

        }
        public int Id { get; set; }
        public DateTime RentalDate { get; set; }


    }
}
