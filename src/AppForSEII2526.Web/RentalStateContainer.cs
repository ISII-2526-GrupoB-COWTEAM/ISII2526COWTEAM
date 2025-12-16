using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class RentalStateContainer
    {

        //we create an instance of Rental when an instance of RentalStateContainer is created
        public RentalForCreateDTO Rental { get; private set; } = new RentalForCreateDTO()
        {
            RentalDevices = new List<RentalDeviceDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
        public decimal TotalPrice
        {
            get
            {
                int numberOfDays = (Rental.RentalDateTo - Rental.RentalDateFrom).Days;
                return Convert.ToDecimal(Rental.RentalDevices.Sum(ri => ri.PriceForRent * numberOfDays));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddDeviceToRental(DeviceForRentalDTO device)
        {
            //before adding a movie we checked whether it has been already added
            if (!Rental.RentalDevices.Any(ri => ri.DeviceID == device.Id))
                //we add it if it is not in the list
                Rental.RentalDevices.Add(new RentalDeviceDTO()
                {
                    DeviceID = device.Id,
                    Name = device.Name,
                    Brand = device.Brand,
                    Color = device.Color,
                    Year = device.Year,
                    Model = device.Model,
                    PriceForRent = device.PriceForRent,
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveRentalDeviceToRent(RentalDeviceDTO item)
        {
            Rental.RentalDevices.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Rental.RentalDevices.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Rental = new RentalForCreateDTO()
            {
                RentalDevices = new List<RentalDeviceDTO>()
            };
        }
    }
}