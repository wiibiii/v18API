using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Blog
{
    //[Keyless]
    public class BlogPostTags
    {
        
        public long BlogPostsId { get; set; }
        public string TagsId { get; set; }
    }
}
