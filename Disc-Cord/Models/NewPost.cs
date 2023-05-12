namespace Disc_Cord.Models
{
    public class NewPost
    {
        public int Id { get; set; }
        public virtual Subforum? Subforum { get; set; }
        public int SubForumId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public bool Reported { get; set; }
        public int? LikeCounter { get; set; }
        public string? Image { get; set; }
        public List<Models.Comment>? Comments { get; set; }
        public DateTime Date { get; set; }
        public List<Models.Like>? Likes { get; set; }
        public List<Models.NewPostLike>? NewPostLikes { get; set; }
    }
}
