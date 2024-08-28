import { Component, OnInit } from '@angular/core';
import { BlogsService } from '../blogs.service';
import { Home } from '../../shared/models/blogs/home';
import { BlogPost } from '../../shared/models/blogs/blogPost';
import { Tags } from '../../shared/models/blogs/tag';

@Component({
  selector: 'app-bloghome',
  templateUrl: './bloghome.component.html',
  styleUrl: './bloghome.component.css',
})
export class BloghomeComponent implements OnInit {
  home: Home | undefined;
  blogs: BlogPost[] = [];
  tags: Tags[] = [];
  errorMessages: string[] = [];

  totalPages = 0;
  pageNumber = 0;
  pageSize = 3;
  nextPage = 0;
  previousPage = 0;
  searchQuery: string = '';
  sortBy: string = '';
  sortDirection: string = '';

  constructor(private blogService: BlogsService) {}

  ngOnInit(): void {
    this.getBlogs('', 1);
  }

  getBlogs(searchQuery: string, pageNo: number = 1) {
    this.blogService.getAllBlogs(searchQuery, pageNo).subscribe({
      next: (blogs: any | undefined) => {
        //console.log(blogs);
        // if (blogs.blogPosts.length > 0) {
        //   this.home.blogPosts = blogs.blogPosts;
        // }
        this.blogs = blogs.blogPosts;
        this.tags = blogs.tags;
        this.totalPages = blogs.totalPages;
        this.pageNumber = blogs.pageNumber;
        this.pageSize = blogs.pageSize;
        this.nextPage = this.pageNumber + 1;
        this.previousPage = this.pageNumber - 1;

        // console.log('Author: ' + this.home.BlogPosts[0].author);
      },
      error: (error) => {
        if (error.error.errors) {
          this.errorMessages = error.error.errors;
        } else {
          this.errorMessages.push(error.error);
        }
      },
    });
  }
}
