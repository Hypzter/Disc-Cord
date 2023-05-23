using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Helper;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Disc_Cord.Pages
{
    public class PostCommentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostCommentModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Models.NewPost Post { get; set; }


        [BindProperty]
        public Models.Report Report { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }


        [BindProperty]
        public Models.Comment NewComment { get; set; }


        [BindProperty]
        public Models.NewPostLike NewPostLike { get; set; }


        [BindProperty]
        public Models.CommentLike NewCommentLike { get; set; }


        public List<ApplicationUser> AllUsers { get; set; }
        public ApplicationUser MyUser { get; set; }

        [BindProperty]
        public string EditText { get; set; }

        [BindProperty]
        public string EditPostText { get; set; }
        public bool IsAdmin { get; set; }


        private static int _id;
        public async Task<IActionResult> OnGetAsync(int id, string userid, int postid, int commentid,
            int deletepostid, int deletecommentid, bool deletebool, int unflagpostid, int unflagcommentid)
        {
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser != null)
			{
				IsAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
			}

			if (id != 0)
            {
                _id = id;
            }

            Post = await _context.NewPost.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == _id);
            AllUsers = await _context.ApplicationUsers.ToListAsync();
            MyUser = AllUsers.Where(x => x.Id == Post.UserId).SingleOrDefault();
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
                List<Models.Report> deletePostReport = await _context.Reports.Where(x => x.PostId == deletepostid).ToListAsync();
                List<Models.Comment> reportedComments = await _context.Comment.Where(x => x.NewPostId == deletepostid && x.Reported == true).ToListAsync();
                List<Models.Report> deleteCommentReport = new();
                foreach (var report in reportedComments)
                {
                    deleteCommentReport.AddRange(_context.Reports.Where(x => x.CommentId == report.Id).ToList());
                }



                if (deletePost != null)
                {
                    _context.NewPost.Remove(deletePost);
                    if (deletePostReport != null)
                    {
                        _context.Reports.RemoveRange(deletePostReport);
                        if (deleteCommentReport != null)
                        {
                            _context.Reports.RemoveRange(deleteCommentReport);
                        }
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Forum");
                }
                deletebool = false;

            }

            if (deletecommentid != 0 && deletebool == true)
            {
                Models.Comment deleteComment = await _context.Comment.FindAsync(deletecommentid);
                List<Models.Report> deleteReport = await _context.Reports.Where(x => x.CommentId == deletecommentid).ToListAsync();


                if (deleteComment != null)
                {
                    _context.Comment.Remove(deleteComment);
                    if (deleteReport != null)
                    {
                        _context.Reports.RemoveRange(deleteReport);

                    }
                    await _context.SaveChangesAsync();
                }
                deletebool = false;
            }
            
            if (unflagpostid != 0)
            {
                var post = await _context.NewPost.Where(p => p.Id == unflagpostid).FirstOrDefaultAsync();
                post.Reported = false;
                var reportedPost = await _context.Reports.Where(r => r.PostId == unflagpostid).ToListAsync();

                _context.Reports.RemoveRange(reportedPost);
                await _context.SaveChangesAsync();
            }
            if (unflagcommentid != 0)
            {
                var comment = await _context.Comment.Where(p => p.Id == unflagcommentid).FirstOrDefaultAsync();
                comment.Reported = false;
                var reportedComment = await _context.Reports.Where(r => r.CommentId == unflagcommentid).ToListAsync();

                _context.Reports.RemoveRange(reportedComment);
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int editpostid, int editcommentid, bool editbool, bool newcommentbool, int reportpostid, int reportcommentid)
        {
            if (reportpostid != 0)
            {
                Report.PostId = reportpostid;
                _context.NewPost.FirstOrDefault(x => x.Id == reportpostid).Reported = true;
                _context.Reports.Add(Report);
                await _context.SaveChangesAsync();
            }
            if (reportcommentid != 0)
            {
                Report.CommentId = reportcommentid;
                _context.Comment.FirstOrDefault(x => x.Id == reportcommentid).Reported = true;
                _context.Reports.Add(Report);
                await _context.SaveChangesAsync();
            }

            if (editcommentid != 0 && editbool == true)
            {
                Models.Comment editComment = await _context.Comment.FindAsync(editcommentid);


                if (editComment != null)
                {
                    editComment.Text = EditText;
                    await _context.SaveChangesAsync();
                }
                editbool = false;

            }

            if (editpostid != 0 && editbool == true)
            {
                Models.NewPost editPost = await _context.NewPost.FindAsync(editpostid);


                if (editPost != null && EditPostText != null)
                {
                    editPost.Text = EditPostText;
                    await _context.SaveChangesAsync();
                }
                editbool = false;
            }



            if (newcommentbool == true)
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
                if(NewComment.Text == null)
                {
                    NewComment.Text = "Kommentar utan innehåll";
                }
                NewComment.Text = Helper.HelperMethods.CensorText(NewComment.Text);
                _context.Add(NewComment);
                await _context.SaveChangesAsync();

                newcommentbool = false;
            }

            string url = "./PostComment?id=" + _id.ToString();
            return Redirect(url);
        }
    }
}
