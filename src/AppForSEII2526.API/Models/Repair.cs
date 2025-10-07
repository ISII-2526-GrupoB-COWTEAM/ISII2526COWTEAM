namespace AppForSEII2526.API.Models
{
    public class Repair
    {

        public Repair()
        {
        }   

        public Repair(float cost, string description, int id, string name, int scaleId, Scale scale, IList<ReceiptItem> receiptItems)
        {
            Cost = cost;
            Description = description;
            Id = id;
            Name = name;
            ScaleId = scaleId;
            Scale = scale;
            ReceiptItems = receiptItems;
        }

        public float Cost { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ScaleId { get; set; }
        
        [Required]
        public Scale Scale { get; set; }

        public IList<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    }
}
