namespace API.Models.Blog
{
    public class BlogPostComment
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long BlogPostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
