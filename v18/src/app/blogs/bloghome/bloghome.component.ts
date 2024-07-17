import { Component, OnInit } from '@angular/core';
import { BlogsService } from '../blogs.service';
import { Home } from '../../shared/models/blogs/home';
import { BlogPost } from '../../shared/models/blogs/blogPost';

@Component({
  selector: 'app-bloghome',
  templateUrl: './bloghome.component.html',
  styleUrl: './bloghome.component.css',
})
export class BloghomeComponent implements OnInit {
  home: Home | undefined;
  blogs: BlogPost[] = [];
  constructor(private blogService: BlogsService) {}

  ngOnInit(): void {
    this.getBlogs();
  }

  getBlogs() {
    this.blogService.getAllBlogs().subscribe({
      next: (blogs: Home | undefined) => {
        console.log(blogs);
        // if (blogs.blogPosts.length > 0) {
        //   this.home.blogPosts = blogs.blogPosts;
        // }
        this.home = blogs;

        // console.log('Author: ' + this.home.BlogPosts[0].author);
      },
    });
  }
}
