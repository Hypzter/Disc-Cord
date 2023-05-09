using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
    public class ForumPostModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ForumPostModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public Models.Subforum Subforum { get; set; }

        [BindProperty]
        public Models.NewPost NewPost { get; set; }
        public async Task OnGetAsync(int id)
        {
            Subforum = await _context.Subforum.Include(x => x.NewPosts).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.Add(NewPost);
            string postCommentId = "./PostComment?id=" + NewPost.Id;
            await _context.SaveChangesAsync();

            // Fixa s� r�tt ID kommer med
            return RedirectToPage(postCommentId);
        }
    }
}
