using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;

namespace Dot.Net.WebApi.Repositories
{
    public class RatingService
    {
        private IDbContext _context { get; }

        public RatingService(IDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> AddRating(Rating rating)
        {
            var _rating = new Rating
            {
                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating,
                OrderNumber = rating.OrderNumber
            };
            _context.Ratings.Add(_rating);
            await _context.SaveChangesAsync();
            return _rating;
        }

        public async Task<bool> UpdateRatingById(int id, Rating rating)
        {
            var _rating = _context.Ratings.Find(id);
            if (_rating == null)
            {
                return false;
            }

            _rating.MoodysRating = rating.MoodysRating;
            _rating.SandPRating = rating.SandPRating;
            _rating.FitchRating = rating.FitchRating;
            _rating.OrderNumber = rating.OrderNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRatingById(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating == null)
            {
                return false; // Or throw an exception
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
