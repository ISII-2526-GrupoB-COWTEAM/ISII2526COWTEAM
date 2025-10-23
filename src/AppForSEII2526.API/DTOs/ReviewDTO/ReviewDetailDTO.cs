namespace AppForSEII2526.API.DTOs.ReviewDTO
{
    public class ReviewDetailDTO : ReviewForCreateDTO
    {
        public ReviewDetailDTO(int id, string applicationUserName, string applicationUserCountry,
            string reviewTitle, DateTime reviewDate, IList<ReviewItemDTO> reviewItems)
            : base(applicationUserName,
                  applicationUserCountry, 
                  reviewTitle,
                  reviewItems)
        {
            Id = id;
            ReviewDate = reviewDate;
        }
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
