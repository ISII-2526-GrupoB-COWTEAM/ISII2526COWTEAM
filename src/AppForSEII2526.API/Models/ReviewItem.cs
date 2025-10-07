namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    [PrimaryKey(nameof(DeviceId), nameof(ReviewId))]

    public class ReviewItem
    {
        public ReviewItem()
        {
        }

        public ReviewItem(Device device, Review review)
        {
            Device = device;
            DeviceId = device.Id;
            Review = review;
            ReviewId = review.ReviewId;
        }

        public ReviewItem(Device device, Review review, string comments, int rating) : this(device, review)
        {
            Comments = comments;
            Rating = rating;
        }

        // Relación con Device
        public Device Device { get; set; }
        public int DeviceId { get; set; }

        // Relación con Review
        public Review Review { get; set; }
        public int ReviewId { get; set; }

        [StringLength(250, ErrorMessage = "Comments cannot exceed 250 characters.")]
        public string Comments { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
    }
}
