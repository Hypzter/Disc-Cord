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
        [BindProperty]
		public Models.Message NewMessage { get; set; }
        private static int _messageid;
		public async Task OnGet(int messageid)
        {
            if(messageid != 0)
            {
                _messageid = messageid;
            }
            Message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == _messageid);
            if(!Message.IsRead) Message.IsRead = true;
            await _context.SaveChangesAsync();
        }

		public async Task<IActionResult> OnPostAsync()
		{
			_context.Messages.Add(NewMessage);
			_context.SaveChanges();
			return RedirectToPage("./InboxMessage");

		}
	}
}
