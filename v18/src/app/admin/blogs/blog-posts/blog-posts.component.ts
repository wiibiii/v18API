import { Component, OnInit } from '@angular/core';
import { BlogsService } from '../../../blogs/blogs.service';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BlogPost } from '../../../shared/models/blogs/blogPost';
import { Tag } from '../../../shared/models/blogs/tag';
import { SharedService } from '../../../shared/shared.service';
import { AddBlogPostRequest } from '../../../shared/models/blogs/addBlogPostRequest';

@Component({
  selector: 'app-blog-posts',
  templateUrl: './blog-posts.component.html',
  styleUrl: './blog-posts.component.css',
})
export class BlogPostsComponent implements OnInit {
  blogPostForm: FormGroup = new FormGroup({});
  formInitialized = false;
  submitted = false;
  errorMessages: string[] = [];
  allTags: Tag[] = [];

  get tags(): FormControl {
    return this.blogPostForm.get('tags') as FormControl;
  }

  constructor(
    private router: Router,
    private activedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private blogService: BlogsService,
    private sharedService: SharedService
  ) {}

  ngOnInit(): void {
    const id = this.activedRoute.snapshot.paramMap.get('id');

    if (id) {
    } else {
      this.initializeForm(undefined);
    }

    this.getAllBlogTags();
  }

  initializeForm(tag: BlogPost | undefined) {
    this.blogPostForm = this.formBuilder.group({
      id: [''],
      heading: ['', Validators.required],
      pageTitle: ['', Validators.required],
      content: ['', Validators.required],
      shortDescription: ['', Validators.required],
      featuredImageUrl: ['', Validators.required],
      urlHandle: ['', Validators.required],
      publishedDate: ['', Validators.required],
      author: ['', Validators.required],
      visible: [true, Validators.required],
      tags: ['', Validators.required],
    });

    this.formInitialized = true;
  }

  getAllBlogTags() {
    this.blogService.getAllBlogTags().subscribe({
      next: (tags: Tag[]) => {
        this.allTags = tags;
      },
    });
  }

  submit() {
    this.submitted = true;
    this.errorMessages = [];
    if (this.blogPostForm.valid) {
      this.blogService.addBlogPost(this.blogPostForm.value).subscribe({
        next: (_) => {
          this.sharedService.showNotification(
            true,
            'Blog Added',
            'A new blog post has been added.'
          );
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
}
