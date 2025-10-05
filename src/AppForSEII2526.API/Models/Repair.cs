namespace AppForSEII2526.API.Models
{
    public class Repair
    {

        public float Cost { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ScaleId { get; set; }   

        public Scale Scale { get; set; }

        public IList<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    }
}
