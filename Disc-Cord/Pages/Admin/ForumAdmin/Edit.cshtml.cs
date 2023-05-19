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
using Microsoft.AspNetCore.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using System.Reflection.Metadata;

namespace Disc_Cord.Pages.Admin.ForumAdmin
{
    public class EditModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public EditModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Forum Forum { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Forum == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum.FirstOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }
            Forum = forum;
            return Page();
        }



        ////------------------- API CALL ----------------------//

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var forum = await DataManager.DataManager.GetForumById(id.Value);
        //    if (forum == null)
        //    {
        //        return NotFound();
        //    }

        //    Forum = forum;
        //    return Page();
        //}



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(Forum.Id))
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


        ////------------------- API CALL ----------------------//

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    DataManager.DataManager dataManager = new();
        //    bool updateResult = await dataManager.UpdateForum(Forum);
        //    if (updateResult)
        //    {
        //        return RedirectToPage("./Index");
        //    }
        //    else
        //    {
        //        return StatusCode((int)HttpStatusCode.InternalServerError);
        //    }
        //}


        private bool ForumExists(int id)
        {
          return (_context.Forum?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
