using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;

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
        public List<Models.Comment> Comments { get; set; }

        public List<Object> Activities { get; set; }


        [BindProperty]
		public Models.Message Message { get; set; }

        public bool IsAdmin { get; set; }

		private static string _userId;


        public async Task OnGetAsync(string userid)
        {
            if (userid != null)
            {
                _userId = userid;
            }

            User = await _userManager.Users.Where(u => u.Id == _userId).FirstOrDefaultAsync();
            IsAdmin = await _userManager.IsInRoleAsync(User, "Admin");

            await RefreshActivities();
        }

        private async Task RefreshActivities()
        {
            var posts = await _context.NewPost.Where(p => p.UserId == User.Id).ToListAsync();
            var comments = await _context.Comment.Where(c => c.UserId == User.Id).ToListAsync();

            Activities = new List<object>();
            Activities.AddRange(posts.Select(p => new { Date = p.Date, Text = p.Header, Id = p.Id, PostOrComment = p.Header }));
            Activities.AddRange(comments.Select(c => new { Date = c.Date, Text = c.Text, Id = c.NewPostId }));
            Activities = Activities.OrderByDescending(item => ((DateTime)item.GetType().GetProperty("Date").GetValue(item, null))).Take(10).ToList();
        }



        public async Task<IActionResult> OnPostAsync()
		{


			if (Message.Text != null)
			{
                if (Message.Headline == null)
                {
                    Message.Headline = "Ingen rubrik";
                }
                Message.Timestamp = DateTime.Now;
				await _context.Messages.AddAsync(Message);
				await _context.SaveChangesAsync();

			}
			string url = "./UserPage?userid=" + _userId;
			return Redirect(url);

		}

    }
}
