
namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewForCreateDTO
    {
        public ReviewForCreateDTO(string applicationUserName, string applicationUserCountry, string reviewTitle, IList<ReviewItemDTO> reviewItems)
        {
            ApplicationUserName = applicationUserName ?? throw new ArgumentNullException(nameof(applicationUserName));
            ApplicationUserCountry = applicationUserCountry ?? throw new ArgumentNullException(nameof(applicationUserCountry));
            ReviewTitle = reviewTitle ?? throw new ArgumentNullException(nameof(reviewTitle));
            ReviewItems = reviewItems ?? throw new ArgumentNullException(nameof(reviewItems));
        }

        public ReviewForCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>();
        }

        public string ApplicationUserName { get; set; }
        [Required]
        public string ApplicationUserCountry { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public IList<ReviewItemDTO> ReviewItems { get; set; }

        [Display(Name = "Rating")]
        public int OverallRating
        {
            get{return (int)ReviewItems.Average(ri => ri.Rating);}
        }
    }
}
