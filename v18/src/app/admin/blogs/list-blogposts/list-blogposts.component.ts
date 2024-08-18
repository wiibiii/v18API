import { Component, OnInit, TemplateRef } from '@angular/core';
import { BlogsService } from '../../../blogs/blogs.service';
import { BlogPost } from '../../../shared/models/blogs/blogPost';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-list-blogposts',
  templateUrl: './list-blogposts.component.html',
  styleUrl: './list-blogposts.component.css',
})
export class ListBlogpostsComponent implements OnInit {
  searchQuery: string = '';
  blogs: BlogPost[] = [];
  blogToDelete: BlogPost | undefined;
  totalPages = 0;
  pageNumber = 0;
  pageSize = 3;
  nextPage = 0;
  previousPage = 0;
  modalRef?: BsModalRef;
  sortBy: string = '';
  constructor(
    private blogService: BlogsService,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {
    this.getAllBlogs('', '', '', 3, 1);
  }

  getAllBlogs(
    searchQuery: string,
    sortBy: string,
    sortDirection: string,
    pageSize: number = 3,
    pageNumber = 1
  ) {
    this.blogService
      .getAllBlogsPaginated(
        searchQuery,
        sortBy,
        sortDirection,
        pageSize,
        pageNumber
      )
      .subscribe({
        next: (res: any) => {
          //console.log(res);
          this.blogs = res.value.blogs;
          this.totalPages = res.value.totalPages;
          this.pageNumber = res.value.pageNumber;
          this.pageSize = res.value.pageSize;
          this.nextPage = this.pageNumber + 1;
          this.previousPage = this.pageNumber - 1;
        },
      });
  }

  sortByAHeader(sortBy: string, sortDirection: string) {
    this.sortBy = sortBy;
    this.getAllBlogs(
      this.searchQuery.trim(),
      sortBy,
      sortDirection,
      this.pageSize,
      this.pageNumber
    );
  }

  deleteBlog(id: string, template: TemplateRef<any>) {
    let blog = this.findBlog(id);
    if (blog) {
      this.blogToDelete = blog;
      this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    }
  }

  confirm() {}

  decline() {
    this.blogToDelete = undefined;
    this.modalService?.hide();
  }

  private findBlog(id: string): BlogPost | undefined {
    let blog = this.blogs.find((x) => x.id === id);
    if (blog) return blog;
    return undefined;
  }
}
