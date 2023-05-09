using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
    public class PostCommentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PostCommentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.NewPost Post { get; set; }

        [BindProperty]
        public Models.Comment NewComment { get; set; }
        public async Task OnGetAsync(int id)
        {
            Post = await _context.NewPost.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
