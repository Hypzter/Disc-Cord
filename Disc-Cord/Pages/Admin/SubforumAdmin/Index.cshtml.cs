using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Data;
using Disc_Cord.Models;

namespace Disc_Cord.Pages.Admin.SubforumAdmin
{
    public class IndexModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public IndexModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Subforum> Subforum { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Subforum != null)
            {
                Subforum = await _context.Subforum
                .Include(s => s.Forum).ToListAsync();
            }
        }
    }
}
