using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class Rating
    {
        // TODO: Map columns in data table RATING with corresponding fields
        public int Id { get; set; }

        [Required(ErrorMessage = "Missing MoodysRating")]
        public string MoodysRating { get; set; }

        [Required(ErrorMessage = "Missing SandPRating")]
        public string SandPRating { get; set; }

        [Required(ErrorMessage = "Missing FitchRating")]
        public string FitchRating { get; set; }

        public byte? OrderNumber { get; set; }
    }
}