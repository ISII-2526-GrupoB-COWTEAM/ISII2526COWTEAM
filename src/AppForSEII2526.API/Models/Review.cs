namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        //Falta implementar el atributo CustomerCount... que aparece truncado
        //En la imagen del diagrama de clases para este caso de uso.
        public Review()
        {
        }

        public Review(int customerId, DateTime dateOfReview, int overallRating, string reviewTitle, IList<ReviewItem> reviewItems)
        {
            //CustomerCount...
            CustomerId = customerId;
            OverallRating = (reviewItems != null && reviewItems.Count > 0)
                ? (int)reviewItems.Average(ri => ri.Rating)
                : overallRating;

            DateOfReview = dateOfReview;
            ReviewTitle = reviewTitle;
            ReviewItems = reviewItems ?? new List<ReviewItem>();
        }

        public Review(int customerId, DateTime dateOfReview, int overallRating, string reviewTitle)
        {
            //CustomerCount...
            CustomerId = customerId;
            OverallRating = overallRating;
            DateOfReview = dateOfReview;
            ReviewTitle = reviewTitle;
            ReviewItems = new List<ReviewItem>();
        }

        public int ReviewId { get; set; }

        //Restriccion
        //public ??? CustomerCount... { get; set; }

        public int CustomerId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Of Review")]
        public DateTime DateOfReview { get; set; }

        [Range(1, 5, ErrorMessage = "Overall rating must be between 1 and 5")]
        public int OverallRating { get; set; }

        [StringLength(100, ErrorMessage = "Review title cannot be longer than 100 characters.")]
        public string ReviewTitle { get; set; }

        // Relación: un Review tiene muchos ReviewItems
        public IList<ReviewItem> ReviewItems { get; set; } = new List<ReviewItem>();
    }
}
