﻿namespace API.Models.Blog
{
    public class BlogPostLike
    {
        public long Id { get; set; }
        public long BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
