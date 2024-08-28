﻿using API.Models.Blog;

namespace API.Models.ViewModel.Blog
{
    public class BlogDetailsViewModel
    {
        public long Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public IEnumerable<BlogPostTag> Tags { get; set; }

        public int TotalLikes { get; set; }

        public bool Liked { get; set; }

        public string CommentDescription { get; set; }
        public IEnumerable<BlogComment> Comments { get; set; }
    }
}
