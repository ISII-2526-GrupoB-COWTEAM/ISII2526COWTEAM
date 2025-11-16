
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

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   ApplicationUserName == dTO.ApplicationUserName &&
                   ApplicationUserCountry == dTO.ApplicationUserCountry &&
                   ReviewTitle == dTO.ReviewTitle &&
                   ReviewItems.SequenceEqual(dTO.ReviewItems) &&
                   OverallRating == dTO.OverallRating &&
                   Id == dTO.Id &&
                   ReviewDate == dTO.ReviewDate;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(ApplicationUserName, ApplicationUserCountry, ReviewTitle, ReviewItems, OverallRating, Id, ReviewDate);
        }
    }
}
