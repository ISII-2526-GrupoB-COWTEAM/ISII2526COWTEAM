using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace AppForSEII2526.API.Models
{
    public class Receipt
    {

        public Receipt() 
        { 
        }

        public Receipt(int Id, string CustomerNameSurname, IList<ReceiptItem> ReceiptItem, string DeliveryAddress,PaymentMethodTypes PaymentMethod, DateTime ReceiptDate, double TotalPrice)
        {
            this.Id = Id;
            this.CustomerNameSurname = CustomerNameSurname;
            this.PaymentMethod = PaymentMethod;
            this.ReceiptDate = ReceiptDate;
            this.TotalPrice = TotalPrice;
            this.DeliveryAddress = DeliveryAddress;
            this.ReceiptItems = ReceiptItem;
        }



        public string CustomerNameSurname { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }

        public int Id { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date Of Review")]
        public DateTime ReceiptDate { get; set; }

        public double TotalPrice { get; set; }

        public IList<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    }
}
