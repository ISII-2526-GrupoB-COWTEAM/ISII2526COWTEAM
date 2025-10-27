using AppForSEII2526.API.DTOs.ReviewDTO;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.ReceiptDTO
{
    public class ReceiptForCreate
    {

        public ReceiptForCreate(string applicationUserName, string applicationUserSurname,
            string deliveryAddress, IList<ReceiptItemDTO> receiptItems)
        {
            ApplicationUserName = applicationUserName ?? throw new ArgumentNullException(nameof(applicationUserName));
            ApplicationUserSurname = applicationUserSurname ?? throw new ArgumentNullException(nameof(applicationUserSurname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            ReceiptItems = receiptItems ?? throw new ArgumentNullException(nameof(receiptItems));
        }
        [Required]
        public string ApplicationUserName { get; set; }
        
        [Required]
        public string ApplicationUserSurname { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public IList<ReceiptItemDTO> ReceiptItems { get; set; }


        public ReceiptForCreate()
        {
            ReceiptItems = new List<ReceiptItemDTO>();
        }

    }
}
