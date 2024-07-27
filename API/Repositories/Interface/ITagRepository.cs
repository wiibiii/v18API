using API.Models.Blog;

namespace API.Repositories.Interface
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllPaginatedAsync(string? searchQuery = null, string? sortBy = null, string? sortDirection = null, int pageNumber = 100, int pageSize = 1);

        Task<List<Tag?>> GetAllBlogTags();

        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);

        Task<int> CountAsync();
    }
}
