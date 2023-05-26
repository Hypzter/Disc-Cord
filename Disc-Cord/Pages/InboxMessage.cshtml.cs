using Disc_Cord.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
    public class InboxMessageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<Models.ApplicationUser> _userManager;
        public InboxMessageModel(ApplicationDbContext context, UserManager<Models.ApplicationUser> userManager)
        {
            _context = context;            
			_userManager = userManager;
        }
        public Models.Message Message { get; set; }
        [BindProperty]
		public Models.Message NewMessage { get; set; }
        private static int _messageid;
		public async Task<IActionResult> OnGetAsync(int messageid, int deletemessageid)
        {
            if(messageid != 0)
            {
                _messageid = messageid;
            }
            Message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == _messageid);
            if(!Message.IsRead) Message.IsRead = true;
            await _context.SaveChangesAsync();

			if (deletemessageid != 0)
			{
				var deleteMessage = await _context.Messages.FindAsync(deletemessageid);
				var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Message.ReceiverId);

				_context.Messages.Remove(deleteMessage);
				await _context.SaveChangesAsync();
				var redirectUrl = "/Inbox?userid=" + user.Id;
				return Redirect(redirectUrl);
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			_context.Messages.Add(NewMessage);
			_context.SaveChanges();
			return RedirectToPage("./InboxMessage");

		}
	}
}
