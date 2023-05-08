using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disc_Cord.Data;
using Disc_Cord.Models;

namespace Disc_Cord.Pages.Admin.ForumAdmin
{
    public class CreateModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public CreateModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Forum Forum { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Forum == null || Forum == null)
            {
                return Page();
            }

            _context.Forum.Add(Forum);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
