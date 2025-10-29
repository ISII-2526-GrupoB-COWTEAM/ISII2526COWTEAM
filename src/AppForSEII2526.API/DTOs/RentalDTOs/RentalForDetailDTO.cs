using AppForSEII2526.API.DTOs.RentalDTO;

namespace AppForSEII2526.API.DTOs.RentalDTOs
{
    public class RentalForDetailDTO : RentalForCreateDTO
    {
        public RentalForDetailDTO(int id, string applicationUserName,string applicationUserSurname, string deliveryAddress, DateTime rentalDate, double totalPrice, DateTime rentalDateFrom, DateTime rentalDateTo, IList<RentalDeviceDTO> rentalDevices)
            : base( applicationUserName, applicationUserSurname, deliveryAddress, rentalDevices)
        {
            Id = id;
            RentalDate = rentalDate;

        }
        public int Id { get; set; }

       
        public DateTime RentalDate { get; set; }


        


    }
}
