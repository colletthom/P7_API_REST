using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        //public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$", ErrorMessage = "The password must contain at least one uppercase, one lowercase, and one number.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Missing FullName")]
        public string FullName { get; set; }

        public string Role { get; set; }
    }
}