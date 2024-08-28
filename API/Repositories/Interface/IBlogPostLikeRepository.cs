using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikesAsync(Guid blogPostId);
        Task<List<BlogPostLike>> GetTotalLikesAsyncBySp(long blogPostId);

        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);

        Task<BlogPostLike> AddLikeForBlogBySp(BlogPostLike blogPostLike);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlogAsync(Guid blogPostId);
    }
}
