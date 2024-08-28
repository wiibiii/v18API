using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsyncBySP();

        Task<IEnumerable<BlogPost>> GetAllPaginatedAsyncBySP(string? searchQuery, string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100);

        Task<BlogPost?> GetAsync(long id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);

        Task<BlogPost> GetByUrlHandleAsyncBySp(string urlHandle);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);

        Task<int> AddAsyncBySP(BlogPost blogPost);
    }
}
