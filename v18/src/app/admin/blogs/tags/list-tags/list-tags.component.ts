import { Component, OnInit, TemplateRef } from '@angular/core';
import { AdminService } from '../../../admin.service';
import { Tag, Tags } from '../../../../shared/models/blogs/tag';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { BlogsService } from '../../../../blogs/blogs.service';
import { SharedService } from '../../../../shared/shared.service';

@Component({
  selector: 'app-list-tags',
  templateUrl: './list-tags.component.html',
  styleUrl: './list-tags.component.css',
})
export class ListTagsComponent implements OnInit {
  constructor(
    private adminService: AdminService,
    private modalService: BsModalService,
    private blogService: BlogsService,
    private sharedService: SharedService
  ) {}

  blogTags: Tags[] = [];
  tagToDelete: Tags | undefined;
  totalPages = 0;
  pageNumber = 0;
  pageSize = 3;
  nextPage = 0;
  previousPage = 0;
  modalRef?: BsModalRef;
  searchQuery: string = '';
  sortBy: string = '';
  sortDirection: string = '';

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

    this.blogService
      .getAllBlogTagsPaginated(
        searchQuery,
        sortBy,
        sortDirection,
        pageSize,
        pageNumber
      )
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
        error: (error) => {
          console.log(error);
        },
      });
  }

  searchTag() {
    this.getAllTags(
      this.searchQuery.trim(),
      this.sortBy,
      this.sortDirection,
      this.pageSize,
      this.pageNumber
    );
  }

  soryBy(sortBy: string, sortDirection: string) {
    this.sortBy = sortBy;
    this.getAllTags(
      this.searchQuery.trim(),
      sortBy,
      sortDirection,
      this.pageSize,
      this.pageNumber
    );
  }

  deleteTag(id: string, template: TemplateRef<any>) {
    let tag = this.findTag(id);
    if (tag) {
      this.tagToDelete = tag;
      this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
    }
  }

  confirm() {
    if (this.tagToDelete) {
      this.blogService.deleteBlogTag(this.tagToDelete.id).subscribe({
        next: (res: any) => {
          this.sharedService.showNotification(
            true,
            'Deleted',
            `The Tag ${this.tagToDelete?.name} has been deleted`
          );

          this.blogTags = this.blogTags.filter(
            (x) => x.id !== this.tagToDelete?.id
          );

          this.tagToDelete = undefined;
          this.modalRef?.hide();

          this.getAllTags('', '', '', 3, 1);
        },
      });
    }
  }

  decline() {
    this.tagToDelete = undefined;
    this.modalService?.hide();
  }

  private findTag(id: string): Tags | undefined {
    let tag = this.blogTags.find((x) => x.id === id);
    if (tag) return tag;
    return undefined;
  }
}
