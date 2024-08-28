using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostLikeRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await bloggieDbContext.BlogPostLike.AddAsync(blogPostLike);
            await bloggieDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<BlogPostLike> AddLikeForBlogBySp(BlogPostLike blogPostLike)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                    blogPostLike.UserId,
                    blogPostLike.BlogPostId
                };

                var retFromDb = await conn.QueryAsync<BlogPostLike>("[add_BlogPostLike]", parameters, commandType: CommandType.StoredProcedure);

                if (retFromDb != null) return retFromDb.FirstOrDefault();

                return null;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlogAsync(Guid blogPostId)
        {
            //return await bloggieDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalLikesAsync(Guid blogPostId)
        {
            //return await bloggieDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
            throw new NotImplementedException();
        }

        public async Task<List<BlogPostLike>> GetTotalLikesAsyncBySp(long blogPostId)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {
                    blogPostId
                };

                var retFromDb = await conn.QueryAsync<BlogPostLike>("[sel_AllBlogPostLike]", parameters, commandType: CommandType.StoredProcedure);

                if (retFromDb != null) return retFromDb.ToList();

                return null;
            }
            catch (Exception)
            {
                //todo: create logs
                throw;
            }
        }
    }
}
