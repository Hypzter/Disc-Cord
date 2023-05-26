using Disc_Cord.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
    public class InboxMessageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public InboxMessageModel(ApplicationDbContext context)
        {
            _context = context;            
        }
        public Models.Message Message { get; set; }
        public async Task OnGet(int messageid)
        {
            Message = await _context.Messages.FirstOrDefaultAsync(x => x.Id  == messageid);
        }
    }
}
