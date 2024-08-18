using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class TagRepository : ITagRepositoryRepository
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

        public async Task<Tag> AddAsyncTagBySP(Tag tag)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                    tag.Name,
                    tag.DisplayName
                };

                var retFromDb = await conn.QueryFirstOrDefaultAsync<Tag>("[add_tag]", parameters, commandType: CommandType.StoredProcedure);


                return retFromDb;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> CountAsync()
        {
            return await bloggieDbContext.Tags.CountAsync();
        }

        public async Task<int> CountAsyncBySp()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<Tags?> DeleteAsyncBySP(long id)
        {


            var existingTag = await GetAsync(id);

            if (existingTag != null)
            {
                try
                {
                    using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                    var parameters = new
                    {
                        id
                    };

                    var retFromDb = await conn.QueryFirstOrDefaultAsync<Tags>("[del_Tag]", parameters, commandType: CommandType.StoredProcedure);

                    if (retFromDb != null) return retFromDb;

                    return null;
                }
                catch (Exception)
                {
                    return null;                    
                }

                
            }

            return null;
        }

        public async Task<IEnumerable<Tags>> GetAllPaginatedAsyncBySP(string? searchQuery, string? sortBy, string? sortDirection, int pageNumber = 1, int pageSize = 100)
        {

            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                    pageNumber = pageNumber,
                };

                var retFromDb = await conn.QueryAsync<Tags>("[sel_AllTagsPaginated]", parameters, commandType: CommandType.StoredProcedure);

                //return await bloggieDbContext.Tags.ToListAsync();
                //var query = bloggieDbContext.Tags.AsQueryable();
                var query = retFromDb.AsQueryable();


                //filtering
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(x => x.Name.ToLower().Contains(searchQuery) ||
                                            x.DisplayName.ToLower().Contains(searchQuery));
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
                //var skipResults = (pageNumber - 1) * pageSize;
                //query = query.Skip(skipResults).Take(pageSize);

                //return await query.ToListAsync();
                return query;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Tag>> GetAllBlogTags()
        {
            return await bloggieDbContext.Tags.ToListAsync();
        }

        public async Task<List<Tag>> GetAllBlogTagsBySP()
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());


                var retFromDb = await conn.QueryAsync<Tags>("[sel_AllTagsNonPaginated]", null, commandType: CommandType.StoredProcedure);

                if (retFromDb != null) 
                    return retFromDb.Select( x =>new Tag { Id = x.Id, DisplayName = x.DisplayName, Name = x.Name}).ToList();

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task<Tag?> GetAsync(Guid id)
        public async Task<Tags?> GetAsync(long id)
        {
            //var tag = await bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            //if (tag != null)
            //{
            //    return tag;
            //}

            //return null;

            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                    id
                };

                var retFromDb = await conn.QueryFirstOrDefaultAsync<Tags>("[sel_Tag]", parameters, commandType: CommandType.StoredProcedure);

                if(retFromDb != null)  return retFromDb;

                return null;
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<Tags?> UpdateAsyncBySP(Tag tag)
        {
            //var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);
            var existingTag = await GetAsync(tag.Id);

            if (existingTag != null)
            {
                try
                {
                    using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                    var parameters = new
                    {
                        tag.Id,
                        tag.Name,
                        tag.DisplayName,
                    };

                    var retFromDb = await conn.QueryFirstOrDefaultAsync<Tags>("[upd_Tag]", parameters, commandType: CommandType.StoredProcedure);

                    if (retFromDb != null) { return retFromDb; }

                    return null;
                }
                catch (Exception)
                {

                    return null;
                }                
            }

            return null;

        }
    }
}
