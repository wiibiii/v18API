namespace API.Models.ViewModel.Blog
{
    public class AddLikeRequest
    {
        public long BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
