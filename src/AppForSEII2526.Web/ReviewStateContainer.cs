using AppForSEII2526.Web.API;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {
        // Creamos una instancia vacía de la reseña
        public ReviewForCreateDTO Review { get; private set; } = new ReviewForCreateDTO()
        {
            ReviewItems = new List<ReviewItemDTO>()
        };

        // Cálculo automático del rating (media de items)
        public int OverallRating
        {
            get
            {
                if (Review.ReviewItems == null || !Review.ReviewItems.Any())
                {
                    return 0;
                }
                
                return (int)Math.Round(Review.ReviewItems.Average(ri => ri.Rating));
            }
        }
        // Evento para notificar cambios a la UI
        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();


    public void AddDeviceToReview(DeviceForReviewDTO device)
        {
            if (!Review.ReviewItems.Any(pi => pi.DeviceID == device.Id))
                Review.ReviewItems.Add(new ReviewItemDTO()
                {
                    DeviceID = device.Id,
                    Name = device.Name,
                    Model = device.Model,
                    Year = device.Year,
                    Rating = 1,  // Default rating
                    Comments = ""
                }
            );
        }


        public void RemoveReviewItem(ReviewItemDTO item)
        {
            Review.ReviewItems.Remove(item);
        }


        public void ClearReviewCart()
        {
            Review.ReviewItems.Clear();
        }


        public void ReviewProcessed()
        {
            Review = new ReviewForCreateDTO()
            {
                ReviewItems = new List<ReviewItemDTO>()
            };
        }
    }
}
