using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages.Admin.ReportedAdmin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Models.Report> Reported { get; set; }
        public List<Models.Comment> Comments { get; set; }
        public List<Models.NewPost> NewPosts { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Reported = await _context.Reports.ToListAsync();
            Comments = await _context.Comment.ToListAsync();
            NewPosts = await _context.NewPost.ToListAsync();

            return Page();
        }
    }
}
