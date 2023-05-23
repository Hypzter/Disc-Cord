using Disc_Cord.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Disc_Cord.Pages
{
	public class MessagesModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		private readonly UserManager<Models.ApplicationUser> _userManager;
		public MessagesModel(ApplicationDbContext context, UserManager<Models.ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public List<Models.Message> Messages { get; set; }
		public List<Models.ApplicationUser> Users { get; set; }
		[BindProperty]
		public Models.Message NewMessage { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);
			Users = await _userManager.Users.ToListAsync();

			if (_context.Messages != null)
			{
				var messages = await _context.Messages
					.Where(m => m.ReceiverId == userId)
					.OrderByDescending(m => m.Timestamp)
					.ToListAsync();

				Messages = messages;
			}



			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{

			NewMessage.Timestamp = DateTime.Now;


			_context.Messages.Add(NewMessage);
			_context.SaveChanges();
			return RedirectToPage("./Messages");

		}
	}
}
