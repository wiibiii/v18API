import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../admin.service';
import { Tag } from '../../../../shared/models/blogs/tag';

@Component({
  selector: 'app-list-tags',
  templateUrl: './list-tags.component.html',
  styleUrl: './list-tags.component.css',
})
export class ListTagsComponent implements OnInit {
  constructor(private adminService: AdminService) {}

  blogTags: Tag[] = [];
  totalPages = 0;
  pageNumber = 0;
  pageSize = 3;
  nextPage = 0;
  previousPage = 0;
  ngOnInit(): void {
    this.getAllTags('', '', '', 3, 1);
  }

  getAllTags(
    searchQuery: string,
    sortBy: string,
    sortDirection: string,
    pageSize: number = 3,
    pageNumber = 1
  ) {
    //this.blogTags = [];
    this.adminService
      .getAllBlogTags(searchQuery, sortBy, sortDirection, pageSize, pageNumber)
      .subscribe({
        next: (tags: any) => {
          //console.log(tags);
          this.blogTags = tags.value.tags;
          this.totalPages = tags.value.totalPages;
          this.pageNumber = tags.value.pageNumber;
          this.pageSize = tags.value.pageSize;
          this.nextPage = this.pageNumber + 1;
          this.previousPage = this.pageNumber - 1;
        },
      });
  }
}
