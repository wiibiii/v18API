using API.Models.Blog;
using API.Models.ViewModel.Blog;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBlogPostController : ControllerBase
    {
        private readonly ITagRepositoryRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostTagsRepository blogPostTagsRepository;

        public AdminBlogPostController(
            ITagRepositoryRepository tagRepository, 
            IBlogPostRepository blogPostRepository,
            IBlogPostTagsRepository blogPostTagsRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
            this.blogPostTagsRepository = blogPostTagsRepository;
        }

        [HttpPost("admin-add-blog")]
        public async Task<IActionResult> AdminAddBlog(AddBlogPostRequest addBlogPostRequest)
        {
            var blog = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible                
            };

            //map tags from selected tags
            var selectedTags = new List<BlogPostTags>();
            var allTags = await tagRepository.GetAllBlogTags();
            var tagsToBeAdded = allTags.Where(item => addBlogPostRequest.Tags.Contains(item.Name)).ToList();
            //TODO:
            

            //blog.Tags = selectedTags;

            //await blogPostRepository.AddAsync(blog);
            await blogPostRepository.AddAsyncBySP(blog);

            foreach (var tag in tagsToBeAdded)
            {
                //selectedTags.Add(new BlogPostTags { BlogPostsId = blog.Id, TagsId = tag.Id });
                //await blogPostTagsRepository.AddBlogPostTagsAsync(new BlogPostTags { BlogPostsId = blog.Id, TagsId = tag.Id });
            }

            

            return Ok();

        }
    }
}
