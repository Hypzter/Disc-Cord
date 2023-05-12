namespace Disc_Cord.Models
{
    public class CommentLike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual Comment? Comment { get; set; }
        public int CommentId { get; set; }
    }
}
