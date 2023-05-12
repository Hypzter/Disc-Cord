using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Helper;
using System.ComponentModel.DataAnnotations;

namespace Disc_Cord.Pages
{
    public class PostCommentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PostCommentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.NewPost Post { get; set; }


        [BindProperty]
        public IFormFile UploadedImage { get; set; }


        [BindProperty]
        public Models.Comment NewComment { get; set; }


        [BindProperty]
        public Models.NewPostLike NewPostLike { get; set; }



        private static int _id;
        public async Task<IActionResult> OnGetAsync(int id, string userid, int postid)
        {
            if (id != 0)
            {
                _id = id;
            }

            Post = await _context.NewPost.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == _id);
            NewPostLike = await _context.NewPostLike.Where(u => u.UserId == userid && u.NewPostId == postid).SingleOrDefaultAsync();
            if (postid != 0)
            {

                var listoflikes = _context.NewPostLike.Where(p => p.NewPostId == postid).ToList();
                var usercheck = listoflikes.FirstOrDefault(u => u.UserId == userid);
                if (usercheck == null)
                {
                    NewPostLike = new Models.NewPostLike
                    {
                        NewPostId = postid,
                        UserId = userid
                    };
                    _context.Add(NewPostLike); 
                    await _context.SaveChangesAsync();
                }
                var listoflikes2 = _context.NewPostLike.Where(p => p.NewPostId == postid).ToList();
                Post.LikeCounter = listoflikes2.Count;
                await _context.SaveChangesAsync();

            }
            return Page();
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
            NewComment.Image = fileName;
            _context.Add(NewComment);
            string url = "./PostComment?id=" + _id.ToString();
            await _context.SaveChangesAsync();

            return Redirect(url);

        }
    }
}
