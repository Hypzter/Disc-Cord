using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{

    public class ForumModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ForumModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Models.Forum> Forums { get; set; }


        public async Task OnGetAsync()
        {
            Forums = await _context.Forum.Include(x => x.SubForums).ThenInclude(x => x.NewPosts).ThenInclude(x => x.Comments).ToListAsync();
        }
    }
}
