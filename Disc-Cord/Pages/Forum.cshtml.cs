using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Buffers.Text;
using System.Security.Policy;

namespace Disc_Cord.Pages
{

    public class ForumModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ForumModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Models.Forum> Forums { get; set; }

        public IList<Models.Subforum> Subforums { get; set; } = default!;



        [BindProperty(SupportsGet = true)]
        public string? ChooseableTopics { get; set; }



        public SelectList? Topics { get; set; }



        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }


        //public async Task OnGetAsync()
        //{
        //    Forums = await _context.Forum.Include(x => x.SubForums).ThenInclude(x => x.NewPosts).ThenInclude(x => x.Comments).ToListAsync();


        //    IQueryable<string> search = from m in _context.Subforum
        //                                orderby m.Name
        //                                select m.Name;

        //    var topics = from m in _context.Subforum
        //                 select m;

        //    var posts = from m in _context.NewPost
        //                select m;

        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        topics = topics.Where(t => t.NewPosts.Any(np => np.Header.Contains(SearchString)));
        //    }

        //    if (!string.IsNullOrEmpty(ChooseableTopics))
        //    {
        //        topics = topics.Where(x => x.Name == ChooseableTopics);
        //    }
        //    //Topics = new SelectList(await search.Distinct().ToListAsync());
        //    Topics = new SelectList(await topics.Select(t => t.Name).Distinct().ToListAsync());
        //    Subforums = await topics.ToListAsync();


        //}
        //public async Task OnGetAsync()
        //{
        //    Forums = await _context.Forum.Include(x => x.SubForums).ThenInclude(x => x.NewPosts).ThenInclude(x => x.Comments).ToListAsync();

        //    IQueryable<string> search = from m in _context.Subforum
        //                                orderby m.Name
        //                                select m.Name;

        //    var topics = from m in _context.Subforum
        //                 select m;

        //    var posts = from m in _context.NewPost
        //                select m;

        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        topics = topics.Where(t => t.NewPosts.Any(np => np.Header.Contains(SearchString)));
        //        search = search.Where(s => s.Contains(SearchString));
        //    }

        //    if (!string.IsNullOrEmpty(ChooseableTopics))
        //    {
        //        topics = topics.Where(x => x.Name == ChooseableTopics);
        //    }

        //    Topics = new SelectList(await search.Distinct().ToListAsync());
        //    Subforums = await topics.ToListAsync();
        //}
        public async Task OnGetAsync()
        {
            Forums = await _context.Forum.Include(x => x.SubForums).ThenInclude(x => x.NewPosts).ThenInclude(x => x.Comments).ToListAsync();

            var topics = from m in _context.Subforum
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                topics = topics.Where(t => t.NewPosts.Any(np => np.Header.ToLower().Contains(SearchString.ToLower())));
                Topics = new SelectList(await topics.SelectMany(t => t.NewPosts).Where(np => np.Header.Contains(SearchString.ToLower())).Select(np => np.Header).Distinct().ToListAsync());
            }
            else
            {
                Topics = new SelectList(await topics.SelectMany(t => t.NewPosts).Select(np => np.Header.ToLower()).Distinct().ToListAsync());
            }

            Subforums = await topics.ToListAsync();
        }
    }
}
