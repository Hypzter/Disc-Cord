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
        public List<Models.Like>? Likes { get; set; }


        private static int _id;
        public async Task OnGetAsync(int id, string userid, int postid)
        {
            if (_id == 0)
            {
                _id = id;
            }
            Post = await _context.NewPost.Include(x => x.Comments).ThenInclude(x => x.Likes).FirstOrDefaultAsync(x => x.Id == _id);

            if (postid != 0)
            {
                if (Post.Likes == null)
                {
                    Post.Likes = new List<Models.Like>();
                    Post.Likes.Add(new Models.Like { UserId = userid });
                    Post.LikeCounter++;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    foreach (var like in Post.Likes)
                    {
                        if (like.UserId != userid)
                        {
                            Post.Likes.Add(new Models.Like { UserId = userid });
                            Post.LikeCounter++;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
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
