using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Helper;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using System.Drawing.Printing;
using System.Security.Policy;

namespace Disc_Cord.Pages
{
    public class PostCommentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration Configuration;

        public PostCommentModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            Configuration = configuration;
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

        //public List<Models.NewPostLike>  { get; set; }

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


        public PaginatedList<Models.Comment> Comments { get; set; }


        public async Task<IActionResult> OnGetAsync(int id, string userid, int postid, int commentid,
            int deletepostid, int deletecommentid, bool deletebool, int unflagpostid, int unflagcommentid, int? pageIndex)
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

            var pageSize = Configuration.GetValue("PageSize", 10);

            Comments = await PaginatedList<Models.Comment>.CreateAsync(
                _context.Comment.Where(x => x.NewPostId == _id)
                , pageIndex ?? 1, pageSize);



            Post = await _context.NewPost.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == _id);
            AllUsers = await _context.ApplicationUsers.ToListAsync();
            MyUser = AllUsers.Where(x => x.Id == Post.UserId).SingleOrDefault();
            NewPostLike = await _context.NewPostLike.Where(u => u.UserId == userid && u.NewPostId == postid).SingleOrDefaultAsync();
            NewComment = await _context.Comment.Where(c => c.Id == commentid).FirstOrDefaultAsync();
            NewCommentLike = await _context.CommentLike.Where(u => u.UserId == userid && u.CommentId == commentid).SingleOrDefaultAsync();

            if (postid != 0)
            {
                await AddPostLikesAsync(postid, userid);
                return Page();
            }

            if (commentid != 0)
            {
                string redirectUrl = await AddCommentLikeAsync(commentid, userid);
                return Redirect(redirectUrl);
            }


            if (deletepostid != 0 && deletebool == true)
            {
                await DeletePostAsync(deletepostid, deletebool);
                return Redirect("./ForumPost");
            }

            if (deletecommentid != 0 && deletebool == true)
            {
                await DeleteCommentAsync(deletecommentid, deletebool);

            }
            if (unflagcommentid != 0 || unflagpostid != 0)
            {
                string redirectUrl = await UnFlagAsync(unflagpostid, unflagcommentid);
                return Redirect(redirectUrl);

            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int editpostid, int editcommentid, bool editbool, bool newcommentbool, int reportpostid, int reportcommentid)
        {
            if (reportpostid != 0 || reportcommentid != 0)
            {
                string redirectUrl = await ReportAsync(reportpostid, reportcommentid);
                return Redirect(redirectUrl);
            }

            if (editcommentid != 0 && editbool == true)
            {
                string redirectUrl = await EditCommentAsync(editcommentid, editbool);
                return Redirect(redirectUrl);
            }

            if (editpostid != 0 && editbool == true)
            {
                await EditPostAsync(editpostid, editbool);
            }

            if (newcommentbool == true)
            {
                string redirectUrl = await AddNewcommentAsync(newcommentbool);
                return Redirect(redirectUrl);
            }

            string url = "./PostComment?id=" + _id.ToString();
            return Redirect(url);
        }

        private async Task<string> AddNewcommentAsync(bool newcommentbool)
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
            if (NewComment.Text == null)
            {
                NewComment.Text = "Kommentar utan innehåll";
            }
            NewComment.Text = Helper.HelperMethods.CensorText(NewComment.Text);
            _context.Add(NewComment);
            _context.SaveChanges();

            newcommentbool = false;

            var redirectUrl = string.Empty;
            int commentPageNumber = 0;
            var list = _context.Comment.Where(x => x.NewPostId == _id).ToList();
            var comment = list.Where(x => x.Id == NewComment.Id).FirstOrDefault();
            var index = list.IndexOf(comment);
            for (int i = 0; i < index + 1; i += Helper.Variables.PageSize)
            {
                commentPageNumber++;
            }
            if (commentPageNumber == 1)
            {
                redirectUrl = ("/PostComment?id=" + comment.NewPostId + "#" + comment.Id).ToString();

            }
            else
            {
                redirectUrl = ("/PostComment?pageIndex=" + commentPageNumber + "&id=" + comment.NewPostId + "#" + comment.Id).ToString();
            }
            return redirectUrl;
        }

        private async Task EditPostAsync(int editpostid, bool editbool)
        {
            Models.NewPost editPost = await _context.NewPost.FindAsync(editpostid);


            if (editPost != null && EditPostText != null)
            {
                editPost.Text = EditPostText;
                editbool = false;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> EditCommentAsync(int editcommentid, bool editbool)
        {
            Models.Comment editComment = await _context.Comment.FindAsync(editcommentid);


            if (editComment != null)
            {
                editComment.Text = EditText;
                await _context.SaveChangesAsync();
            }
            editbool = false;

            var redirectUrl = string.Empty;
            int commentPageNumber = 0;
            var list = _context.Comment.Where(x => x.NewPostId == _id).ToList();
            var index = list.IndexOf(editComment);
            for (int i = 0; i <= index + 1; i += Helper.Variables.PageSize)
            {
                commentPageNumber++;
            }
            if (commentPageNumber == 1)
            {
                redirectUrl = ("/PostComment?id=" + _id + "#" + editComment.Id).ToString();

            }
            else
            {
                redirectUrl = ("/PostComment?pageIndex=" + commentPageNumber + "&id=" + _id + "#" + editComment.Id).ToString();
            }
            return redirectUrl;
        }

        private async Task<string> ReportAsync(int reportpostid, int reportcommentid)
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

                var redirectUrl = string.Empty;
                int commentPageNumber = 0;
                var list = _context.Comment.Where(x => x.NewPostId == _id).ToList();
                var comment = list.Where(x => x.Id == reportcommentid).FirstOrDefault();
                var index = list.IndexOf(comment);
                for (int i = 0; i < index + 1; i += Helper.Variables.PageSize)
                {
                    commentPageNumber++;
                }
                if (commentPageNumber == 1)
                {
                    redirectUrl = ("/PostComment?id=" + comment.NewPostId + "#" + comment.Id).ToString();

                }
                else
                {
                    redirectUrl = ("/PostComment?pageIndex=" + commentPageNumber + "&id=" + comment.NewPostId + "#" + comment.Id).ToString();
                }
                return redirectUrl;
            }
            string url = "./PostComment?id=" + _id.ToString();
            return url;
        }

        private async Task<string> UnFlagAsync(int unflagpostid, int unflagcommentid)
        {
            if (unflagpostid != 0)
            {
                var post = await _context.NewPost.Where(p => p.Id == unflagpostid).FirstOrDefaultAsync();
                post.Reported = false;
                var reportedPost = await _context.Reports.Where(r => r.PostId == unflagpostid).ToListAsync();

                _context.Reports.RemoveRange(reportedPost);
                await _context.SaveChangesAsync();

            }
            else if (unflagcommentid != 0)
            {
                var comment = await _context.Comment.Where(p => p.Id == unflagcommentid).FirstOrDefaultAsync();
                comment.Reported = false;
                var reportedComment = await _context.Reports.Where(r => r.CommentId == unflagcommentid).ToListAsync();

                _context.Reports.RemoveRange(reportedComment);
                await _context.SaveChangesAsync();

                var redirectUrl = string.Empty;
                int commentPageNumber = 0;
                var list = _context.Comment.Where(x => x.NewPostId == _id).ToList();
                var index = list.IndexOf(comment);
                for (int i = 0; i < index + 1; i += Helper.Variables.PageSize)
                {
                    commentPageNumber++;
                }
                if (commentPageNumber == 1)
                {
                    redirectUrl = ("/PostComment?id=" + comment.NewPostId + "#" + comment.Id).ToString();

                }
                else
                {
                    redirectUrl = ("/PostComment?pageIndex=" + commentPageNumber + "&id=" + comment.NewPostId + "#" + comment.Id).ToString();
                }
                return redirectUrl;
            }
            string url = "./PostComment?id=" + _id.ToString();
            return url;


        }

        private async Task DeleteCommentAsync(int deletecommentid, bool deletebool)
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

        private async Task DeletePostAsync(int deletepostid, bool deletebool)
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
                deletebool = false;
            }
            

        }

        private async Task<string> AddCommentLikeAsync(int commentid, string userid)
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
                await _context.AddAsync(NewCommentLike);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.CommentLike.Remove(usercheck);
                await _context.SaveChangesAsync();

            }

            var numberOfLikes = _context.CommentLike.Where(p => p.CommentId == commentid).ToList();
            NewComment.LikeCounter = numberOfLikes.Count;
            await _context.SaveChangesAsync();

            var redirectUrl = string.Empty;
            int commentPageNumber = 0;
            var list = _context.Comment.Where(x => x.NewPostId == _id).ToList();
            var comment = list.Where(x => x.Id == commentid).FirstOrDefault();
            var index = list.IndexOf(comment);
            for (int i = 0; i < index + 1; i += Helper.Variables.PageSize)
            {
                commentPageNumber++;
            }
            if (commentPageNumber == 1)
            {
                redirectUrl = ("/PostComment?id=" + comment.NewPostId + "#" + comment.Id.ToString());

            }
            else
            {
                redirectUrl = ("/PostComment?pageIndex=" + commentPageNumber + "&id=" + comment.NewPostId + "#" + comment.Id).ToString();
            }
            return redirectUrl;

        }

        private async Task AddPostLikesAsync(int postid, string userid)
        {
            var listOfLikes = await _context.NewPostLike.Where(p => p.NewPostId == postid).ToListAsync();
            var usercheck = listOfLikes.FirstOrDefault(u => u.UserId == userid);
            if (usercheck == null)
            {
                NewPostLike = new Models.NewPostLike
                {
                    NewPostId = postid,
                    UserId = userid
                };
                await _context.AddAsync(NewPostLike);
                await _context.SaveChangesAsync();
            }
            else
            {
                var likeToRemove = await _context.NewPostLike.Where(l => l.UserId == userid).FirstOrDefaultAsync();
                _context.NewPostLike.Remove(likeToRemove);
                await _context.SaveChangesAsync();

            }
            var numberOfLikes = await _context.NewPostLike.Where(p => p.NewPostId == postid).ToListAsync();
            Post.LikeCounter = numberOfLikes.Count;
            await _context.SaveChangesAsync();
        }

    }
}
