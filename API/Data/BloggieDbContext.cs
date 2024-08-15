﻿using API.Models.Blog;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<BlogPostTags> BlogPostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }

    
}
