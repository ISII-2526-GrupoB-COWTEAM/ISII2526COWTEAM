namespace AppForSEII2526.API.Models
{

    using System.ComponentModel.DataAnnotations;
    [PrimaryKey(nameof(RepairID), nameof(ReceiptID))]
    public class ReceiptItem
    {
        public ReceiptItem() { }

        public ReceiptItem(Repair Repair, Receipt Receipt) { 
            this.Repair = Repair;
            this.RepairID = Repair.Id;
            this.Receipt = Receipt;
            this.ReceiptID = Receipt.Id;
        }

        public ReceiptItem(Repair Repair, Receipt Receipt, string Model) : this(Repair, Receipt) 
        {
            this.Model = Model;
        }



        [Required]
        public string Model { get; set; }

        //Relacion con Receipt
        public Receipt Receipt { get; set; }
        public int ReceiptID { get; set; }

        //Relacion con Repair
        public Repair Repair { get; set; }
        public int RepairID { get; set; }

    }
}