using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Data;
using Disc_Cord.Models;
using System.Net;

namespace Disc_Cord.Pages.Admin.ForumAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public DeleteModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Forum Forum { get; set; } = default!;

        //public async Task<IActionResult> OnGetAsync(int? id)
        //{
        //    if (id == null || _context.Forum == null)
        //    {
        //        return NotFound();
        //    }

        //    var forum = await _context.Forum.FirstOrDefaultAsync(m => m.Id == id);

        //    if (forum == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        Forum = forum;
        //    }
        //    return Page();
        //}


        //------------------- API CALL ----------------------//
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await DataManager.DataManager.GetForumById(id.Value);
            if (forum == null)
            {
                return NotFound();
            }

            Forum = forum;
            return Page();
        }



        //public async Task<IActionResult> OnPostAsync(int? id)
        //{
        //    if (id == null || _context.Forum == null)
        //    {
        //        return NotFound();
        //    }
        //    var forum = await _context.Forum.FindAsync(id);

        //    if (forum != null)
        //    {
        //        Forum = forum;
        //        _context.Forum.Remove(Forum);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToPage("./Index");
        //}


        //------------------- API CALL ----------------------//
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DataManager.DataManager dataManager = new();
            bool deleteResult = await dataManager.DeleteForum(id.Value);
            if (deleteResult)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


    }
}
