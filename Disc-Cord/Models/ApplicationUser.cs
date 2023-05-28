using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Disc_Cord.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string? FirstName { get; set; }

        [Required]
        [PersonalData]
        public string? LastName { get; set; }

        [Required]
        [PersonalData]
        public string? Alias { get; set; }

        [PersonalData]
        public string? ImageUrl { get; set; }

        [PersonalData]
        public string? AboutMe { get; set; }

        [PersonalData]
        public DateTime DateJoined { get; set; }


    }
}
