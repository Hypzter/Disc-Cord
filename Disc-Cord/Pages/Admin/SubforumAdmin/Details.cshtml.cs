using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Disc_Cord.Pages.Admin.SubforumAdmin
{
    [Authorize(Roles = "Admin")]

    public class DetailsModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public DetailsModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Subforum Subforum { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Subforum == null)
            {
                return NotFound();
            }

            var subforum = await _context.Subforum.FirstOrDefaultAsync(m => m.Id == id);
            if (subforum == null)
            {
                return NotFound();
            }
            else 
            {
                Subforum = subforum;
            }
            return Page();
        }
    }
}
