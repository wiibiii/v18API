using API.Models;
using API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ITagRepositoryRepository tagRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(
            IBlogPostRepository blogPostRepository,
            ITagRepositoryRepository tagRepository,
            IBlogPostLikeRepository blogPostLikeRepository,
            IBlogPostCommentRepository blogPostCommentRepository,
            SignInManager<User> signInManager,
            UserManager<IdentityUser> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.tagRepository = tagRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }        
    }
}
