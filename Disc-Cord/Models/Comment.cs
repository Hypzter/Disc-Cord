﻿using Microsoft.Build.Framework;

namespace Disc_Cord.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public virtual NewPost? NewPost { get; set; }
        public int NewPostId { get; set; }
        [Required]
        public string Text { get; set; }
        public string UserId { get; set; }
        public bool Reported { get; set; }
        public int LikeCounter { get; set; }
        public string? Image { get; set; }
        public DateTime Date { get; set; }

        public List<Models.CommentLike>? CommentLikes { get; set; }
    }
}
