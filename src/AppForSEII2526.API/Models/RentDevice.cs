namespace AppForSEII2526.API.Models
{
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
        public RentDevice(Device device, Rental rent, int quantity, double price) : this(device, rent)
        {
            Quantity = quantity;
            Price = price;
        }
        public RentDevice(int deviceId, Rental rent, int quantity, double price)
        {
            DeviceId = deviceId;
            Rent = rent;
            Quantity = quantity;
            Price = price;
        }
        public RentDevice(int deviceId, int rentalId, int quantity, double price) : this(deviceId, null, quantity, price)
        {
            RentalId = rentalId;
        }
        // Relación con Device
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public int Quantity { get; set; }

        // Relación con Rental
        public Rental Rent { get; set; }
        public int RentalId { get; set; }
        public double Price { get; set; }
    }
}
