using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Domain
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        //public string UserName { get; set; }
        //public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Missing FullName")]
        public string FullName { get; set; }

        public string Role { get; set; }
    }
}