using API.Data;
using API.Models.Blog;
using API.Repositories.Interface;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(
            BloggieDbContext bloggieDbContext
            )
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<int> AddAsyncBySP(BlogPost blogPost)
        {
            try
            {
                using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

                var parameters = new
                {                    
                    blogPost.Heading,
                    blogPost.PageTitle,
                    blogPost.Content,
                    blogPost.ShortDescription,
                    blogPost.FeaturedImageUrl,
                    blogPost.UrlHandle,
                    blogPost.PublishedDate,                    
                    blogPost.Author,
                    blogPost.Visible
                };

                var retFromDb = await conn.QueryFirstOrDefaultAsync<int>("add_blog", parameters, commandType: CommandType.StoredProcedure);

                return retFromDb;


            }
            catch (Exception ex )
            {

                throw;
            }
            

           
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlog != null)
            {
                bloggieDbContext.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();

                return existingBlog;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            using var conn = new SqlConnection(bloggieDbContext.Database.GetConnectionString());

            var ret = await conn.QueryAsync<BlogPost>("sel_AllBlogs", null, commandType: CommandType.StoredProcedure);

            if (ret != null)
            {
                return ret;
            }

            return null;
            //return await bloggieDbContext.BlogPosts.Where(x => x.Visible == true).Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost> GetAsync(Guid id)
        {
            //return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
            return await bloggieDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts
                //.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts
                //.Include(x => x.Tags).
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                //existingBlog.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();

                return existingBlog;
            }

            return null;
        }
    }
}
