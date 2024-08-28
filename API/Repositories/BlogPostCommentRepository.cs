using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await bloggieDbContext.BlogPostComment.AddAsync(blogPostComment);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<BlogPostComment> AddAsyncBySp(BlogPostComment blogPostComment)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {                    
                    blogPostComment.BlogPostId,
                    blogPostComment.UserId,
                    blogPostComment.Description
                };

                var retFromDb = await conn.QueryAsync<BlogPostComment>("[add_BlogpostComment]", parameters, commandType: CommandType.StoredProcedure);

                return retFromDb.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            //return await bloggieDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsyncBySp(long blogPostId)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                   blogPostId
                };

                var retFromDb = await conn.QueryAsync<BlogPostComment>("[sel_BlogPostComments]", parameters, commandType: CommandType.StoredProcedure);

                return retFromDb;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
