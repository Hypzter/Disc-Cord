using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Disc_Cord.Pages
{
	[Authorize]

	public class InboxModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Models.ApplicationUser> _userManager;
        public InboxModel(ApplicationDbContext context, UserManager<Models.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
		[BindProperty]
		public Models.Message NewMessage { get; set; }

		public List<Models.Message> Messages { get; set; }
        public List<Models.ApplicationUser> Users { get; set; }


        public async Task<IActionResult> OnGetAsync(int deletemessageid)
        {
            Users = await _userManager.Users.ToListAsync();
            var user = Users.FirstOrDefault(x => x.Email == User.Identity.Name);


            if (_context.Messages != null)
            {
                var messages = await _context.Messages
                    .Where(m => m.ReceiverId == user.Id)
                    .OrderByDescending(m => m.Timestamp)
                    .ToListAsync();

                Messages = messages;
            }
            if(deletemessageid  != 0)
            {
                string redirectUrl = await DeleteMessageAsync(deletemessageid, user.Id);
				return Redirect(redirectUrl);
			}

            return Page();
        }

		public async Task<IActionResult> OnPostAsync()
		{

			if (NewMessage.Text != null && NewMessage.ReceiverId != null)
			{
				if (NewMessage.Headline == null)
				{
					NewMessage.Headline = "Ingen rubrik";
				}
				NewMessage.Timestamp = DateTime.Now;
				await _context.Messages.AddAsync(NewMessage);
				await _context.SaveChangesAsync();

			}
			return RedirectToPage("./Inbox");


		}

		private async Task<string> DeleteMessageAsync(int deletemessageid, string userid)
		{
			var deleteMessage = await _context.Messages.FindAsync(deletemessageid);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userid);

				_context.Messages.Remove(deleteMessage);
				await _context.SaveChangesAsync();
			var redirectUrl = "/Inbox?userid=" + user.Id;
			return redirectUrl;
		}
	}
}
