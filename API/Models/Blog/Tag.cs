using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Blog
{
    public class Tag
    {
        //public Guid Id { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }        
        //public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
