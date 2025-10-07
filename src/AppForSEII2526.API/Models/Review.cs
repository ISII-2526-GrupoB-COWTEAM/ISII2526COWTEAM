namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        public Review()
        {
        }

        public Review(ApplicationUser applicationUser, DateTime dateOfReview, string reviewTitle, IList<ReviewItem> reviewItems)
        {
            ApplicationUser = applicationUser;

            //Preguntar en tutorias, que es este atributo? se recibe por parametro o es calculado?
            OverallRating = (reviewItems != null && reviewItems.Count > 0)
                ? (int)reviewItems.Average(ri => ri.Rating)
                : 0;
            DateOfReview = dateOfReview;
            ReviewTitle = reviewTitle;
            ReviewItems = reviewItems != null 
                ? new List<ReviewItem>(reviewItems)
                : new List<ReviewItem>();
           
        }

        public int ReviewId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Review")]
        public DateTime DateOfReview { get; set; }

        public int OverallRating { get; set; }

        [StringLength(100, ErrorMessage = "Review title cannot be longer than 100 characters.")]
        public string ReviewTitle { get; set; }

        // Relación: un Review tiene muchos ReviewItems
        public IList<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();
    }
}
