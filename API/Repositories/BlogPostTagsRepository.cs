using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class BlogPostTagsRepository : IBlogPostTagsRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostTagsRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task AddBlogPostTagsAsync(BlogPostTags blogPostTags)
        {

            var parameters = new
            {
                blogPostTags.TagsId,
                blogPostTags.BlogPostsId
            };
            
            using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());
            var ret = await conn.QueryAsync("add_blogtags", parameters, commandType: CommandType.StoredProcedure);
            
            //await bloggieDbContext.BlogPostTags.AddRangeAsync(blogPostTags);
            //await bloggieDbContext.SaveChangesAsync();

            //return blogPostTags;
        }

        public async Task AddBlogPostTagsAsyncBySp(BlogPostTags blogPostTags)
        {

            var parameters = new
            {
                blogPostTags.TagsId,
                blogPostTags.BlogPostsId
            };

            using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());
            var ret = await conn.QueryAsync("add_blogtags", parameters, commandType: CommandType.StoredProcedure);

            //await bloggieDbContext.BlogPostTags.AddRangeAsync(blogPostTags);
            //await bloggieDbContext.SaveChangesAsync();

            //return blogPostTags;
        }

        public async Task<List<BlogPost>> GetAllAsyncPaginatedBySp(int pageNumber)
        {
            try
            {

                var parameters = new
                {
                    pageNumber
                }; 

                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());
                var ret = await conn.QueryAsync<BlogPost>("[sel_AllBlogsPaginated]", parameters, commandType: CommandType.StoredProcedure);

                if (ret != null) return ret.ToList();

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
