import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { BlogsService } from '../blogs.service';
import { BlogPost } from '../../shared/models/blogs/blogPost';
import { AccountService } from '../../account/account.service';
import { take } from 'rxjs';
import { User } from '../../shared/models/account/user';
import { BlogPostComment } from '../../shared/models/blogs/comment';

@Component({
  selector: 'app-blogs-index',
  templateUrl: './blogs-index.component.html',
  styleUrl: './blogs-index.component.css',
})
export class BlogsIndexComponent implements OnInit {
  blogPost!: BlogPost;
  //@ViewChild('liked') liked: ElementRef | undefined;

  commentDescription: string = '';

  constructor(
    private router: Router,
    private activedRoute: ActivatedRoute,
    private blogService: BlogsService,
    public accountService: AccountService
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

    this.router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
        // trick the Router into believing it's last link wasn't previously loaded
        this.router.navigated = false;
        // if you need to scroll back to top, here is the right place
        window.scrollTo(0, 0);
      }
    });
  }

  ngOnInit(): void {
    const urlhandle = this.activedRoute.snapshot.paramMap.get('urlHandle');
    if (urlhandle) {
      this.blogService.getBlogPostByUrlHandle(urlhandle).subscribe({
        next: (res: any) => {
          console.log(res);
          this.blogPost = res;
        },
        error: (error) => {},
      });
    } else {
    }
  }

  onLike(blogpostId: string) {
    this.accountService.user$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        if (user) {
          if (blogpostId) {
            this.blogService.addLikeToBlogpost(blogpostId, user.id).subscribe({
              next: (_) => {
                const i = document.getElementById('liked');
                i?.classList.remove('bi-hand-thumbs-up');
                i?.classList.add('bi-hand-thumbs-up-fill');
              },
            });
          }
        }
      },
    });
  }
  data = {
    commentDescription: '',
    id: '',
    urlHandle: '',
  };
  onCommentSubmit() {
    this.accountService.user$.pipe(take(1)).subscribe({
      next: (user: User | null) => {
        if (user) {
          if (this.blogPost) {
            this.data.id = this.blogPost.id;
            this.data.urlHandle = this.blogPost.urlHandle;
            this.blogService.addCommentToBlogpost(this.data).subscribe({
              next: (response: BlogPostComment) => {
                if (response) {
                  this.router.navigate(['/blogs/index/' + this.data.urlHandle]);
                }
              },
            });
          }
        }
      },
    });
  }
}
