namespace AppForSEII2526.API.DTOs.DeviceDTO
{
    public class DeviceForReviewDTO
    {
        public DeviceForReviewDTO(int id, string name, string brand, string color, int year, string model)
        {
            Id = id;
            Name = name;
            Brand = brand;
            Color = color;
            Year = year;
            Model = model;
        }


        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string Model { get; set; }
    }
}
