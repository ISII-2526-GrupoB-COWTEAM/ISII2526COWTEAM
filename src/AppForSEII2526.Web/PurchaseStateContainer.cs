using AppForSEII2526.Web.API;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {
        // Creamos una instancia vacía de la compra
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO()
        {
            PurchaseDevices = new List<PurchaseDeviceDTO>()
        };

        // Cálculo automático del precio total (suma de items)
        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Purchase.PurchaseDevices.Sum(pi => pi.PurchasePrice * pi.Quantity));
            }
        }
        // Evento para notificar cambios a la UI
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


        public void AddDeviceToPurchase(DeviceForPurchaseDTO device)
        {
            //before adding a movie we checked whether it has been already added
            if (!Purchase.PurchaseDevices.Any(pi => pi.DeviceId == device.Id))
                //we add it if it is not in the list
                Purchase.PurchaseDevices.Add(new PurchaseDeviceDTO()
                {
                    DeviceId = device.Id,
                    Brand = device.Brand,
                    Model = device.Model,
                    Color = device.Colour,
                    PurchasePrice = device.Price,
                    Quantity = 1,
                    Description = null

                }
            );

        }


        public void RemovePurchaseItem(PurchaseDeviceDTO item)
        {
            Purchase.PurchaseDevices.Remove(item);
        }


        public void ClearPurchaseCart()
        {
            Purchase.PurchaseDevices.Clear();
        }


        public void PurchaseProcessed()
        {
            Purchase = new PurchaseForCreateDTO()
            {
                PurchaseDevices = new List<PurchaseDeviceDTO>()
            };
        }
    }
}
