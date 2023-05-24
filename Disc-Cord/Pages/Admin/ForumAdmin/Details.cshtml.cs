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

namespace Disc_Cord.Pages.Admin.ForumAdmin
{
    [Authorize(Roles = "Admin")]

    public class DetailsModel : PageModel
    {
        private readonly Disc_Cord.Data.ApplicationDbContext _context;

        public DetailsModel(Disc_Cord.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
