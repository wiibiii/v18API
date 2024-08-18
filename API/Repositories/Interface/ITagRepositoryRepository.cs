using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface ITagRepositoryRepository
    {
        Task<IEnumerable<Tags>> GetAllPaginatedAsyncBySP(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageNumber = 100, int pageSize = 1);

        Task<List<Tag?>> GetAllBlogTags();
        Task<List<Tag>> GetAllBlogTagsBySP();

        //Task<Tag?> GetAsync(Guid id);
        Task<Tags?> GetAsync(long id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tags?> UpdateAsyncBySP(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

        Task<Tags?> DeleteAsyncBySP(long id);

        Task<int> CountAsync();

        Task<Tag> AddAsyncTagBySP(Tag tag);
    }
}
