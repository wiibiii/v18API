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
    this.adminService
      .getAllBlogTags(searchQuery, sortBy, sortDirection, pageSize, pageNumber)
      .subscribe({
        next: (tags: Tag[]) => {
          console.log(tags);
          this.blogTags = tags;
        },
      });
  }
}
