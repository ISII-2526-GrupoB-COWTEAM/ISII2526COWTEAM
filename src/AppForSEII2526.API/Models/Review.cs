namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        public Review()
        {
        }

        public Review(/*ApplicationUser applicationUser,*/ int customerId, DateTime dateOfReview, int overallRating, string reviewTitle, IList<ReviewItem> reviewItems)
        {
            //Preguntar en tutorias, entiendo que el customerId en verdad es el id de applicationUser
            //ApplicationUser = applicationUser;

            CustomerId = customerId;

            //Preguntar en tutorias, que es este atributo? se recibe por parametro o es calculado?
            OverallRating = (reviewItems != null && reviewItems.Count > 0)
                ? (int)reviewItems.Average(ri => ri.Rating)
                : overallRating;


            DateOfReview = dateOfReview;
            ReviewTitle = reviewTitle;
            ReviewItems = reviewItems ?? new List<ReviewItem>(); 
            
            //Preguntar en tutorias
            /*ReviewItems = reviewItems != null 
                ? new List<ReviewItem>(reviewItems)
                : new List<ReviewItem>();
            */
        }

        public int ReviewId { get; set; }

        //Preguntar tutorias, creo que tienen la misma funcion
        public int CustomerId { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }

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
