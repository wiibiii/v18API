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
    }
}
