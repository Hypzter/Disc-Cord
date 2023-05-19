using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disc_Cord.Data;
using Disc_Cord.Models;
using System.Net;

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

        ////------------------- API CALL ----------------------//
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid || Forum == null)
        //    {
        //        return Page();
        //    }

        //    DataManager.DataManager dataManager = new();
        //    bool createResult = await dataManager.CreateForum(Forum);
        //    if (createResult)
        //    {
        //        return RedirectToPage("./Index");
        //    }
        //    else
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}
    }
}
