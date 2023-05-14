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


        [BindProperty]
        public Models.CommentLike NewCommentLike { get; set; }





		private static int _id;
        public async Task<IActionResult> OnGetAsync(int id, string userid, int postid, int commentid, int deletepostid, int deletecommentid, bool deletebool)
        {
            if (id != 0)
            {
                _id = id;
            }

            Post = await _context.NewPost.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == _id);
            NewPostLike = await _context.NewPostLike.Where(u => u.UserId == userid && u.NewPostId == postid).SingleOrDefaultAsync();
            NewComment = await _context.Comment.Where(c => c.Id == commentid).FirstOrDefaultAsync();
            NewCommentLike = await _context.CommentLike.Where(u => u.UserId == userid && u.CommentId == commentid).SingleOrDefaultAsync();
            if (postid != 0)
            {

                var listOfLikes = _context.NewPostLike.Where(p => p.NewPostId == postid).ToList();
                var usercheck = listOfLikes.FirstOrDefault(u => u.UserId == userid);
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
                var numberOfLikes = _context.NewPostLike.Where(p => p.NewPostId == postid).ToList();
                Post.LikeCounter = numberOfLikes.Count;
                await _context.SaveChangesAsync();

            }

            if (commentid != 0)
            {

                var listOfLikes = _context.CommentLike.Where(p => p.CommentId == commentid).ToList();
                var usercheck = listOfLikes.FirstOrDefault(u => u.UserId == userid);
                if (usercheck == null)
                {
                    NewCommentLike = new Models.CommentLike
                    {
                        CommentId = commentid,
                        UserId = userid
                    };
                    _context.Add(NewCommentLike);
                    await _context.SaveChangesAsync();
                }
                var numberOfLikes = _context.CommentLike.Where(p => p.CommentId == commentid).ToList();
                NewComment.LikeCounter = numberOfLikes.Count;
                await _context.SaveChangesAsync();

            }
            if (deletepostid != 0 && deletebool == true)
            {
                Models.NewPost deletePost = await _context.NewPost.FindAsync(deletepostid);


				if (deletePost != null)
                {
                    _context.NewPost.Remove(deletePost);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Forum");
                }
                deletebool = false;

			}

			if (deletecommentid != 0 && deletebool == true)
			{
				Models.Comment deleteComment = await _context.Comment.FindAsync(deletecommentid);


				if (deleteComment != null)
				{
					_context.Comment.Remove(deleteComment);
					await _context.SaveChangesAsync();
					//return Page();
				}
                deletebool = false;
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
            NewComment.Text = Helper.HelperMethods.CensorText(NewComment.Text);
            _context.Add(NewComment);
            string url = "./PostComment?id=" + _id.ToString();
            await _context.SaveChangesAsync();

            return Redirect(url);

        }
    }
}