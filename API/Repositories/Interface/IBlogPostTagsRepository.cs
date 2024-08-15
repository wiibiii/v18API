using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface IBlogPostTagsRepository
    {
        Task AddBlogPostTagsAsync(BlogPostTags blogPostTags);
    }
}
