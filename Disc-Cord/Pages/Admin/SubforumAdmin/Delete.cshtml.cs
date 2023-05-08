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
    public class DeleteModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public DeleteModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Subforum == null)
            {
                return NotFound();
            }
            var subforum = await _context.Subforum.FindAsync(id);

            if (subforum != null)
            {
                Subforum = subforum;
                _context.Subforum.Remove(Subforum);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
