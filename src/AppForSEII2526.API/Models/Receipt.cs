using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Receipt
    {

        public Receipt() 
        { 
        }

        public Receipt(int Id, ApplicationUser ApplicationUser, IList<ReceiptItem> ReceiptItem, string DeliveryAddress,PaymentMethodTypes PaymentMethod, DateTime ReceiptDate, double TotalPrice)
        {
            this.Id = Id;
            this.ApplicationUser = ApplicationUser;
            this.PaymentMethod = PaymentMethod;
            this.ReceiptDate = ReceiptDate;
            this.TotalPrice = TotalPrice;
            this.DeliveryAddress = DeliveryAddress;
            this.ReceiptItem = ReceiptItem != null
               ? new List<ReceiptItem>(ReceiptItem)
               : new List<ReceiptItem>();
        }


        [Required]
        public ApplicationUser ApplicationUser{ get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }

        public int Id { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date Of Repair")]
        public DateTime ReceiptDate { get; set; }

        public double TotalPrice { get; set; }

        public IList<ReceiptItem> ReceiptItem { get; set; } = new List<ReceiptItem>();

    }
}
