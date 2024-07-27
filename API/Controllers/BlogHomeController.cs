using API.Models.ViewModel.Blog;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogHomeController : ControllerBase
    {
        private readonly ILogger<BlogHomeController> logger;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepository tagRepository;

        public BlogHomeController(
            ILogger<BlogHomeController> logger, 
            IBlogPostRepository blogPostRepository, 
            ITagRepository tagRepository)
        {
            this.logger = logger;
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
        }
        //TODO: add pagination
        [HttpGet("GetBlogs")]
        public async Task<ActionResult<IEnumerable<BlogHomeViewModel>>> Index()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();
            var tags = await tagRepository.GetAllBlogTags();

            var model = new BlogHomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };
            return Ok(model);
        }

        [HttpGet("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
