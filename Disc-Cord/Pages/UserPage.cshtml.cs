using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Disc_Cord.Pages
{
	public class UserPageModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public UserPageModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}


		public Models.ApplicationUser User { get; set; }

		public List<Models.NewPost> Posts { get; set; }

		[BindProperty]
		public Models.Message Message { get; set; }


		private static string _userId;
		public async Task OnGetAsync(string userid)
		{
			if (userid != null)
			{
				_userId = userid;
			}
			User = await _userManager.Users.Where(u => u.Id == _userId).FirstOrDefaultAsync();

			Posts = await _context.NewPost.Where(p => p.UserId == User.Id).Take(5).ToListAsync();
			Posts.OrderBy(p => p.Date);


		}
		public async Task<IActionResult> OnPostAsync()
		{


			if (Message.Text != null)
			{
				await _context.Messages.AddAsync(Message);
				await _context.SaveChangesAsync();

			}
			string url = "./UserPage?userid=" + _userId;
			return Redirect(url);

		}

	}
}
