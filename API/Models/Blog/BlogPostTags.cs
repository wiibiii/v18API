using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Blog
{
    [Keyless]
    public class BlogPostTags
    {
        
        public Guid BlogPostsId { get; set; }
        public Guid TagsId { get; set; }
    }
}
