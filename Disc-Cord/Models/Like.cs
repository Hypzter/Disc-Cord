namespace Disc_Cord.Models
{
	public class Like
	{
		public int Id { get; set; }
        public string UserId { get; set; }
        public virtual NewPost? NewPost { get; set; }
        public int? NewPostId { get; set; }
        public virtual Comment? Comment { get; set; }
        public int? CommentId { get; set; }
    }
}
