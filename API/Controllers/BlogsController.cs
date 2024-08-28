using API.Models;
using API.Models.Blog;
using API.Models.ViewModel.Blog;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepositoryRepository tagRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly IBlogPostTagsRepository blogPostTagsRepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public BlogsController(
            IBlogPostRepository blogPostRepository,
            ITagRepositoryRepository tagRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            IBlogPostCommentRepository blogPostCommentRepository,
            IBlogPostTagsRepository blogPostTagsRepository,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.blogPostTagsRepository = blogPostTagsRepository;
            this.signInManager = signInManager;
            this.userManager = userManager; 
        }

        
        [HttpGet("get-blog-by-urlhandle")]
        public async Task<ActionResult<BlogDetailsViewModel>> GetBlogByUrlHandle(string urlHandle = "")
        {
            if (string.IsNullOrEmpty(urlHandle))
            {
                return null;
            }
            try
            {
                var blogDetailsViewModel = new BlogDetailsViewModel();
                var blogPost = await blogPostRepository.GetByUrlHandleAsyncBySp(urlHandle);
                var liked = false;
                if(blogPost != null)
                {
                    //get all tags for the blogpost
                    var blogPostLikes = await blogPostLikeRepository.GetTotalLikesAsyncBySp(blogPost.Id);

                    var totalLikes = blogPostLikes.Count();
                    if(User.Identity.IsAuthenticated)
                    {
                        //if (signInManager.IsSignedIn(User))
                        //{
                            var userId = userManager.GetUserId(User);
                            if (userId != null)
                            {
                                var likesFromUser = blogPostLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

                                liked = likesFromUser != null;
                            }
                        //}
                    }
                    

                    //get all comments for the blogpost
                    var blogComments = await blogPostCommentRepository.GetCommentsByBlogIdAsyncBySp(blogPost.Id);
                    var blogCommentsForView = new List<BlogComment>();

                    foreach (var blogComment in blogComments)
                    {
                        blogCommentsForView.Add(new BlogComment
                        {
                            Description = blogComment.Description,
                            DateAdded = blogComment.DateAdded,
                            Username = (await userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                        });
                    }

                    //get all tags for the blogpost
                    var tags = await blogPostTagsRepository.GetBlogPostTagByBlogPostIdAsyncBySp(blogPost.Id); 

                    blogDetailsViewModel = new BlogDetailsViewModel
                    {
                        Id = blogPost.Id,
                        Content = blogPost.Content,
                        PageTitle = blogPost.PageTitle,
                        Author = blogPost.Author,
                        FeaturedImageUrl = blogPost.FeaturedImageUrl,
                        Heading = blogPost.Heading,
                        PublishedDate = blogPost.PublishedDate,
                        ShortDescription = blogPost.ShortDescription,
                        UrlHandle = blogPost.UrlHandle,
                        Visible = blogPost.Visible,
                        Tags = tags, //blogPost.Tags,
                        TotalLikes = totalLikes,
                        Liked = liked,
                        Comments = blogCommentsForView
                    };

                    return blogDetailsViewModel;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }

        [HttpPost("add-comment")]
        public async Task<ActionResult<BlogPostComment>> AddComment([FromBody] BlogDetailsViewModel blogDetailsViewModel)
        {
            if(User.Identity.IsAuthenticated)
            {
                try
                {
                    var model = new BlogPostComment
                    {
                        BlogPostId = blogDetailsViewModel.Id,
                        Description = blogDetailsViewModel.CommentDescription,
                        UserId = Guid.Parse(userManager.GetUserId(User)),
                        DateAdded = DateTime.Now
                    };

                    var retFromDB = await blogPostCommentRepository.AddAsyncBySp(model);

                    if (retFromDB != null)
                    {
                        return retFromDB;
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
                
            }

            

            return null;
        }
    }

    public class myreq
    {
        public long Id { get; set; }
        public string CommentDescription { get; set; }
        public string UrlHandle { get; set; }
    }
}
