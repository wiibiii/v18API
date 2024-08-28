using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<BlogPostComment> AddAsyncBySp(BlogPostComment blogPostComment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId);

        Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsyncBySp(long blogPostId);
    }
}
