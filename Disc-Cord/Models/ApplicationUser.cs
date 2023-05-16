using Microsoft.AspNetCore.Identity;

namespace Disc_Cord.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string Alias { get; set; }

        [PersonalData]
        public string ImageUrl { get; set; }


    }
}
