import { Component, OnInit } from '@angular/core';
import { BlogsService } from '../../../blogs/blogs.service';

@Component({
  selector: 'app-blog-posts',
  templateUrl: './blog-posts.component.html',
  styleUrl: './blog-posts.component.css',
})
export class BlogPostsComponent implements OnInit {
  constructor(private blogService: BlogsService) {}

  ngOnInit(): void {}
}
