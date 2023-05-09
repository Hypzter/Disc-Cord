using Disc_Cord.Data;
using Microsoft.EntityFrameworkCore;

namespace Disc_Cord.DataManager
{
    public class DataManager
    {
        private readonly ApplicationDbContext _context;
        public DataManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Models.Forum> Forums { get; set; }

        public async Task<List<Models.Forum>> GetAllForumsAsync()
        {
            Forums = await _context.Forum.ToListAsync();

            return Forums;
        }
    }
}
