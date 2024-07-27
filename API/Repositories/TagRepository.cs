using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public TagRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                bloggieDbContext.Remove(existingTag);
                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllPaginatedAsync(string? searchQuery, string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100)
        {
            //return await bloggieDbContext.Tags.ToListAsync();
            var query = bloggieDbContext.Tags.AsQueryable();

            //filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                        x.DisplayName.Contains(searchQuery));
            }

            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDescending = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDescending ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }
            }

            //pagination
            //skip 0 take 5 -> page 1 of 5 results
            //skip 5 take 5 next 5  -> page 2 of 5 results
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }
        
        public async Task<List<Tag>> GetAllBlogTags()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag != null)
            {
                return tag;
            }

            return null;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await bloggieDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;

        }

        
    }
}
