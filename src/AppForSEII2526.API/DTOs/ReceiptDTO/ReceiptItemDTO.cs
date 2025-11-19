namespace AppForSEII2526.API.DTOs.ReceiptDTO
{
    public class ReceiptItemDTO
    {
        public ReceiptItemDTO(int repairID, string name, string scale, double cost, string model)
        {
            RepairID = repairID;
            Name = name;
            Scale = scale;
            Cost = cost;
            Model = model;
        }

        public int RepairID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Scale { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public string Model { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj is not ReceiptItemDTO other) return false;
            return RepairID == other.RepairID &&
                   Name == other.Name &&
                   Scale == other.Scale &&
                   Cost == other.Cost &&
                   Model == other.Model;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RepairID, Name, Scale, Cost, Model);
        }
    }
}
