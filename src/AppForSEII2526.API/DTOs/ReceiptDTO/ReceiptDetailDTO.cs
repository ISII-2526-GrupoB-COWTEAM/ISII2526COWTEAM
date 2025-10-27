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
    }
}
