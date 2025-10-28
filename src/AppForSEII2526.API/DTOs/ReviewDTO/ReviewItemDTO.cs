namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewItemDTO
    {
        public ReviewItemDTO(int deviceID, string name, string model, int year, int rating, string comments)
        {
            DeviceID = deviceID;
            Name = name;
            Model = model;
            Year = year;
            Rating = rating;
            Comments = comments;
        }

        public int DeviceID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
        [Required]
        public string Comments { get; set; }
    }
}
