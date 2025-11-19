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
        public DateTime RentalDate { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RentalForDetailDTO dTO &&
                   Name == dTO.Name &&
                    Surname == dTO.Surname &&
                        DeliveryAddress == dTO.DeliveryAddress &&
                           PaymentMethod == dTO.PaymentMethod &&
                              RentalDate == dTO.RentalDate &&
                                 RentalDateFrom == dTO.RentalDateFrom &&
                                    RentalDateTo == dTO.RentalDateTo &&
                                      Id == dTO.Id;


        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname, DeliveryAddress, PaymentMethod, RentalDate, RentalDateFrom, RentalDateTo);
        }


    }
}
