using API.Models.Blog;
using API.Models.ViewModel.Blog;

namespace API.Repositories.Interface
{
    public interface IBlogPostTagsRepository
    {
        Task AddBlogPostTagsAsync(BlogPostTags blogPostTags);

        Task AddBlogPostTagsAsyncBySp(BlogPostTags blogPostTags);

        Task<IEnumerable<BlogPostTag>> GetBlogPostTagByBlogPostIdAsyncBySp(long blogPostId);



    }
}
