using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Disc_Cord.Pages
{

    public class ForumModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ForumModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
    }
}
