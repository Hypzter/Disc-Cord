using Disc_Cord.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
	[Authorize]

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
		public Models.ApplicationUser User { get; set; }

		private static int _messageid;
		private static string _userid;

		public async Task<IActionResult> OnGetAsync(int messageid, int deletemessageid)
		{
			if (User != null)
			{
				_userid = User.Id;
			}
			if (messageid != 0)
			{
				_messageid = messageid;
			}
			Message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == _messageid);
			User = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Message.ReceiverId);
			if (!Message.IsRead) Message.IsRead = true;
			await _context.SaveChangesAsync();

			if (deletemessageid != 0)
			{
				var deleteMessage = await _context.Messages.FindAsync(deletemessageid);

				_context.Messages.Remove(deleteMessage);
				await _context.SaveChangesAsync();
				var redirectUrl = "/Inbox?userid=" + _userid;
				return Redirect(redirectUrl);
			}
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == NewMessage.SenderId);

			if (NewMessage.Text != null)
			{
				NewMessage.Timestamp = DateTime.Now;
				//NewMessage.Text = "Från: " + user.Alias + "\n" + "Skickat: " + NewMessage.Timestamp.ToString() + "\n\n" + NewMessage.Text;
				await _context.Messages.AddAsync(NewMessage);
				await _context.SaveChangesAsync();

				var redirectUrl = "/Inbox?userid=" + _userid;
				return Redirect(redirectUrl);
			}
			var url = "/InboxMessage?messageid=" + _messageid;
			return Redirect(url);
		}
	}
}
