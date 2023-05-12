namespace Disc_Cord.Models
{
    public class NewPostLike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual NewPost? NewPost { get; set; }
        public int NewPostId { get; set; }
    }
}
