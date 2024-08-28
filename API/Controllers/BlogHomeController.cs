using API.Models.ViewModel.Blog;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogHomeController : ControllerBase
    {
        private readonly ILogger<BlogHomeController> logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepositoryRepository tagRepository;
        private readonly IBlogPostTagsRepository blogPostTagsRepository;

        public BlogHomeController(
            ILogger<BlogHomeController> logger,
            IBlogPostRepository blogPostRepository,
            ITagRepositoryRepository tagRepository,
            IBlogPostTagsRepository blogPostTagsRepository)
        {
            this.logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            this.blogPostTagsRepository = blogPostTagsRepository;
        }
        //TODO: add pagination
        [HttpGet("GetBlogs")]
        public async Task<IActionResult> Index(
            string searchQuery = "",
            int pageNumber = 1
            )
        {
            try
            {
                var tags = await tagRepository.GetAllBlogTagsBySP();
                var blogs = await blogPostRepository.GetAllPaginatedAsyncBySP("", "", "", pageNumber, 5);
                var totalRecords = blogs.FirstOrDefault().Count;
                var totalPages = Math.Ceiling((decimal)totalRecords / 5);

                if (pageNumber > totalPages)
                {
                    pageNumber--;
                }

                if (pageNumber < 1)
                {
                    pageNumber++;
                }




                foreach (var blog in blogs)
                {
                    var blogtags = await blogPostTagsRepository.GetBlogPostTagByBlogPostIdAsyncBySp(blog.Id);

                    blog.Tags = blogtags.ToList();
                }

                //return Ok(
                //    new JsonResult(new { blogs = blogs,tags = tags, TotalPages = totalPages, PageNumber = pageNumber, PageSize = 5 })
                //);
                var model = new BlogHomeViewModel
                {
                    BlogPosts = blogs,
                    Tags = tags,
                    TotalPages = totalPages,
                    PageNumber = pageNumber
                };

                return Ok(model);
            }
            catch (Exception)
            {

                throw;
            }
            //var blogPosts = await blogPostRepository.GetAllAsyncBySP();
            //var tags = await tagRepository.GetAllBlogTagsBySP();

            //var model = new BlogHomeViewModel
            //{
            //    BlogPosts = blogPosts,
            //    Tags = tags
            //};
            //return Ok(model);
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
