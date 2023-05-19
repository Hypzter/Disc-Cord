using Disc_Cord.Data;
using Disc_Cord.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.Pages
{
    public class ForumPostModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ForumPostModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Models.ApplicationUser> Users { get; set; }

        public Models.Subforum Subforum { get; set; }

        [BindProperty]
        public Models.NewPost NewPost { get; set; }
		[BindProperty]
		public IFormFile UploadedImage { get; set; }

		public async Task OnGetAsync(int id)
        {
            Subforum = await _context.Subforum.Include(x => x.NewPosts).FirstOrDefaultAsync(x => x.Id == id);
            Users = await _context.ApplicationUsers.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
			string fileName = string.Empty;


			if (UploadedImage != null)
			{
				fileName = HelperMethods.RandomString(6) + UploadedImage.FileName;
				var file = "./wwwroot/img/" + fileName;
				using (var fileStream = new FileStream(file, FileMode.Create))
				{
					await UploadedImage.CopyToAsync(fileStream);
				}
			}
			NewPost.Image = fileName;
            if(NewPost.Text == null)
            {
                NewPost.Text = "";
            }
            if(NewPost.Header == null)
            {
                NewPost.Header = "Ny tråd";
            }
			_context.Add(NewPost);
            _context.SaveChanges();
            int id = await _context.NewPost.MaxAsync(p => p.Id);

            string url = "./PostComment?id=" + id.ToString();

            return Redirect(url);

        }
    }
}
