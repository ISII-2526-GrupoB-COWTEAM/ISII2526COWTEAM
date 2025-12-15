using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReceiptStateContainer
    {
        // Creamos una instancia vacía del recibo
        public ReceiptForCreate Receipt { get; private set; } = new ReceiptForCreate()
        {
            ReceiptItems = new List<ReceiptItemDTO>()
        };

        // Cálculo automático del precio total (suma de items)
        public decimal TotalCost
        {
            get
            {
                return Convert.ToDecimal(Receipt.ReceiptItems.Sum(ri => ri.Cost));
            }
        }

        // Evento para notificar cambios a la UI
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddRepairToReceipt(RepairDTO repair)
        {
            // Verificamos si la reparación ya está en el carrito
            if (!Receipt.ReceiptItems.Any(ri => ri.RepairID == repair.Id))
            {
                // La añadimos si no está en la lista usando object initializer
                Receipt.ReceiptItems.Add(new ReceiptItemDTO
                {
                    RepairID = repair.Id,
                    Name = repair.Name,
                    Scale = repair.Scale,
                    Cost = (double)repair.Cost,
                    Model = string.Empty // El modelo se rellenará en CreateRepair
                });
            }
        }

        public void RemoveReceiptItem(ReceiptItemDTO item)
        {
            Receipt.ReceiptItems.Remove(item);
        }

        public void ClearReceiptCart()
        {
            Receipt.ReceiptItems.Clear();
        }

        public void ReceiptProcessed()
        {
            Receipt = new ReceiptForCreate()
            {
                ReceiptItems = new List<ReceiptItemDTO>()
            };
        }
    }
}
