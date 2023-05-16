using Microsoft.AspNetCore.Identity;

namespace Disc_Cord.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
    }
}
