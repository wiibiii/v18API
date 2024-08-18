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
  addNew = false;
  fileName: string = '';
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
      this.addNew = false;
      this.blogService.editBlog(id).subscribe({
        next: (res: BlogPost) => {
          this.formInitialized = true;
          this.initializeForm(res);
        },
      });
    } else {
      this.addNew = true;
      this.initializeForm(undefined);
    }

    this.getAllBlogTags();
  }

  initializeForm(blog: BlogPost | undefined) {
    if (blog) {
      this.blogPostForm = this.formBuilder.group({
        id: [blog.id],
        heading: [blog.heading, Validators.required],
        pageTitle: [blog.pageTitle, Validators.required],
        content: [blog.content, Validators.required],
        shortDescription: [blog.shortDescription, Validators.required],
        featuredImageUrl: [blog.featuredImageUrl, Validators.required],
        urlHandle: [blog.urlHandle, Validators.required],
        publishedDate: [blog.publishedDate, Validators.required],
        author: [blog.author, Validators.required],
        visible: [blog.visible, Validators.required],
        tags: [blog.tags, Validators.required],
      });
    } else {
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
    }

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

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.fileName = input.files[0].name;
    }
  }
}
