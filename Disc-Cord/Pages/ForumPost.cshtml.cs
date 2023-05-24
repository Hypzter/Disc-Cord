using Disc_Cord.Data;
using Disc_Cord.Helper;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Disc_Cord.Pages
{
    public class ForumPostModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public ForumPostModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public List<Models.ApplicationUser> Users { get; set; }

        public Models.Subforum Subforum { get; set; }

        [BindProperty]
        public Models.NewPost NewPost { get; set; }
		[BindProperty]
		public IFormFile UploadedImage { get; set; }

        public PaginatedList<Models.NewPost> NewPosts { get; set; }

        private static int _id;
        public async Task OnGetAsync(int id, int? pageIndex)
        {
            if (id != 0)
            {
                _id = id;
            }

            Subforum = await _context.Subforum.Include(x => x.NewPosts).ThenInclude(x => x.Comments).FirstOrDefaultAsync(x => x.Id == _id);
            Users = await _context.ApplicationUsers.ToListAsync();

            var pageSize = Configuration.GetValue("PageSize", 10);
            NewPosts = await PaginatedList<Models.NewPost>.CreateAsync(
                _context.NewPost.OrderByDescending(x => x.Date).Where(x => x.SubForumId == _id)
                ,pageIndex ?? 1, pageSize);
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
