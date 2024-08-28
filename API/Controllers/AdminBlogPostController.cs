using API.Models.Blog;
using API.Models.ViewModel.Blog;
using API.Repositories.Interface;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBlogPostController : ControllerBase
    {
        private readonly ITagRepositoryRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostTagsRepository blogPostTagsRepository;
        private readonly IImageRepository imageRepository;

        public AdminBlogPostController(
            ITagRepositoryRepository tagRepository,
            IBlogPostRepository blogPostRepository,
            IBlogPostTagsRepository blogPostTagsRepository,
            IImageRepository imageRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
            this.blogPostTagsRepository = blogPostTagsRepository;
            this.imageRepository = imageRepository;
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
            //var allTags = await tagRepository.GetAllBlogTags();
            var allTags = await tagRepository.GetAllBlogTagsBySP();
            var tagsToBeAdded = allTags.Where(item => addBlogPostRequest.Tags.Contains(item.Name)).ToList();
            //TODO:


            //blog.Tags = selectedTags;

            //await blogPostRepository.AddAsync(blog);
            var retFromDb = await blogPostRepository.AddAsyncBySP(blog);

            //foreach (var tag in tagsToBeAdded)
            //{
            //    //selectedTags.Add(new BlogPostTags { BlogPostsId = blog.Id, TagsId = tag.Id });
            //    await blogPostTagsRepository.AddBlogPostTagsAsync(new BlogPostTags { BlogPostsId = blog.Id, TagsId = string.Join(",", tag.Id) });
            //}

            await blogPostTagsRepository.AddBlogPostTagsAsyncBySp(new BlogPostTags { BlogPostsId = retFromDb, TagsId = string.Join(",", tagsToBeAdded.Select(x => x.Id)) });



            return Ok();

        }

        [HttpGet("get-all-blogs-paginated")]
        public async Task<ActionResult<List<BlogPost>>> GetAllBlogsPaginatedBySp(string? searchQuery, string? sortBy, string? sortDirection, int pageSize = 3, int pageNumber = 1)
        {
            try
            {
                var totalRecords = await blogPostRepository.GetAllAsyncBySP();
                var totalPages = Math.Ceiling((decimal)totalRecords.Count() / pageSize);

                if (pageNumber > totalPages)
                {
                    pageNumber--;
                }

                if (pageNumber < 1)
                {
                    pageNumber++;
                }

                var blogs = await blogPostRepository.GetAllPaginatedAsyncBySP(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

                foreach (var blog in blogs)
                {
                    var blogtags = await blogPostTagsRepository.GetBlogPostTagByBlogPostIdAsyncBySp(blog.Id);

                    blog.Tags = blogtags.ToList();

                }
                return Ok(
                new JsonResult(new { blogs = blogs, TotalPages = totalPages, PageNumber = pageNumber, PageSize = pageSize })
                );

            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(long id)
        {


            try
            {
                var blogPost = await blogPostRepository.GetAsync(id);


                var blogtags = await blogPostTagsRepository.GetBlogPostTagByBlogPostIdAsyncBySp(blogPost.Id);

                blogPost.Tags = blogtags.ToList();


                if (blogPost != null)
                {
                    return Ok(blogPost);
                }
            }
            catch (Exception)
            {
                return Ok(null);
            }

            return Ok(null);

        }

        [HttpPost("images")]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageURL = await imageRepository.UploadAsync(file);

            if (imageURL != null)
            {
                return new JsonResult(new { link = imageURL });
            }

            return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
        }
    }
}

