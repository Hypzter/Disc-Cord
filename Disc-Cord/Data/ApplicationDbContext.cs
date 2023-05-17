using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Disc_Cord.Models;

namespace Disc_Cord.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Disc_Cord.Models.Forum> Forum { get; set; } = default!;
        public DbSet<Disc_Cord.Models.Subforum> Subforum { get; set; } = default!;
        public DbSet<Disc_Cord.Models.NewPost> NewPost { get; set; } = default!;
        public DbSet<Disc_Cord.Models.Comment> Comment { get; set; } = default!;
        public DbSet<Disc_Cord.Models.NewPostLike> NewPostLike { get; set; } = default!;
        public DbSet<Disc_Cord.Models.CommentLike> CommentLike { get; set; } = default!;
		public DbSet<Disc_Cord.Models.Message> Messages { get; set; } = default!;
        public DbSet<Disc_Cord.Models.ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Disc_Cord.Models.Report> Reports { get; set; }
    



    }
}