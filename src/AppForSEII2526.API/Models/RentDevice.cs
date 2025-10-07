namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DeviceId), nameof(RentalId))]

    public class RentDevice
    {
         public RentDevice()
        {
        }
        public RentDevice(Device device, Rental rent)
        {
            Device = device;
            DeviceId = device.Id;
            Rent = rent;
            RentalId = rent.Id;
        }
        public RentDevice(Device device, Rental rent, int quantity) : this(device, rent)
        {
            Quantity = quantity;
            Price = device.PriceForRent;
        }
        public RentDevice(int deviceId, Rental rent, double price)
        {
            DeviceId = deviceId;
            Rent = rent;
            Price = price;
        }
        public RentDevice(int deviceId, Rental rent , int quantity, double price) : this(deviceId,rent, price) => Quantity = quantity;
       

        public Device Device { get; set; }
        public int DeviceId { get; set; }

        [StringLength(100, ErrorMessage = "Tienes que seleccionar al menos un dispositivo")]
        public int Quantity { get; set; }

        public Rental Rent { get; set; }
        
        public int RentalId { get; set; }
        public double Price { get; set; }
    }
}

