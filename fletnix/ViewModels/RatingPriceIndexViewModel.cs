using fletnix.Models;

namespace fletnix.ViewModels
{
    public class RatingPriceIndexViewModel
    {
        public Movie Movie { get; set; }
        public double AverageRating { get; set; }
        public double RpRating { get; set; }
    }
}