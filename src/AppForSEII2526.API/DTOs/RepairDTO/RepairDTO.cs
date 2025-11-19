namespace AppForSEII2526.API.DTOs.RepairDTO
{
    public class RepairDTO
    {



        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Scale { get; set; }

        [Required]
        public float Cost { get; set; }

        [Required]
        public string Description { get; set; }

        public RepairDTO(int id, string name, string scale, float cost, string description)
        {
            Id = id;
            Name = name;
            Scale = scale;
            Cost = cost;
            Description = description;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not RepairDTO other)
                return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Scale == other.Scale &&
                   Cost == other.Cost &&
                   Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Scale, Cost, Description);
        }

    }
}
