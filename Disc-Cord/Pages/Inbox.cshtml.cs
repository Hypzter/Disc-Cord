using Disc_Cord.Data;
using Disc_Cord.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Disc_Cord.Pages
{
    public class InboxModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<Models.ApplicationUser> _userManager;
        public InboxModel(ApplicationDbContext context, UserManager<Models.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<Models.Message> Messages { get; set; }
        public List<Models.ApplicationUser> Users { get; set; }
        private string _userid;
        public async Task<IActionResult> OnGetAsync(string userid)
        {
            if (userid != null)
            {
                _userid = userid;
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = await _userManager.FindByIdAsync(userId);
            Users = await _userManager.Users.ToListAsync();

            if (_context.Messages != null)
            {
                var messages = await _context.Messages
                    .Where(m => m.ReceiverId == _userid)
                    .OrderByDescending(m => m.Timestamp)
                    .ToListAsync();

                Messages = messages;
            }



            return Page();
        }
    }
}
