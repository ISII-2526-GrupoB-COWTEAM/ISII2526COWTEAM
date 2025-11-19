using AppForSEII2526.API.DTOs.ReviewDTO;

namespace AppForSEII2526.API.DTOs.ReceiptDTO
{
    public class ReceiptDetailDTO : ReceiptForCreate
    {
        public int Id { get; set; }

        
        public DateTime DateOfReceipt { get; set; }
        public double TotalPrice { get; set; }
       

        public ReceiptDetailDTO(int id, string applicationUserName, string applicationUserSurname,
            string deliveryAddress, DateTime dateOfReceipt, double totalPrice, IList<ReceiptItemDTO> receiptItems):
            base(applicationUserName, applicationUserSurname, deliveryAddress, receiptItems)
        {
            Id = id;
            DateOfReceipt = dateOfReceipt;
            TotalPrice = PrecioTotal(receiptItems);
        }

        public double PrecioTotal(IList<ReceiptItemDTO> receiptItems) { 
            return receiptItems.Sum(ri => ri.Cost);
        }

        public override bool Equals(object obj)
        {
            if (obj is not ReceiptDetailDTO other) return false;
            return Id == other.Id
                && ApplicationUserName == other.ApplicationUserName
                && ApplicationUserSurname == other.ApplicationUserSurname
                && DeliveryAddress == other.DeliveryAddress
                && DateOfReceipt == other.DateOfReceipt
                && TotalPrice == other.TotalPrice
                && ReceiptItems.SequenceEqual(other.ReceiptItems);
        }

        public override int GetHashCode() => HashCode.Combine(Id, ApplicationUserName, ApplicationUserSurname, DeliveryAddress, DateOfReceipt, TotalPrice, ReceiptItems);

    }
}
