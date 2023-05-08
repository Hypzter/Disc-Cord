using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Data;
using Disc_Cord.Models;

namespace Disc_Cord.Pages.Admin.SubforumAdmin
{
    public class EditModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public EditModel(Disc_Cord.Data.ApplicationDbContext context)
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

            var subforum =  await _context.Subforum.FirstOrDefaultAsync(m => m.Id == id);
            if (subforum == null)
            {
                return NotFound();
            }
            Subforum = subforum;
           ViewData["ForumId"] = new SelectList(_context.Forum, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Subforum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubforumExists(Subforum.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SubforumExists(int id)
        {
          return (_context.Subforum?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
