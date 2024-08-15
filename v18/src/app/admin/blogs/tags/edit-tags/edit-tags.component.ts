import { Component, OnInit, TemplateRef } from '@angular/core';
import { AdminService } from '../../../admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EditTag, Tag } from '../../../../shared/models/blogs/tag';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedService } from '../../../../shared/shared.service';
import { BlogsService } from '../../../../blogs/blogs.service';

@Component({
  selector: 'app-edit-tags',
  templateUrl: './edit-tags.component.html',
  styleUrl: './edit-tags.component.css',
})
export class EditTagsComponent implements OnInit {
  tagForm: FormGroup = new FormGroup({});
  formInitialized = false;
  submitted = false;
  errorMessages: string[] = [];
  tagToDelete: EditTag | undefined;
  addNew: boolean = true;
  constructor(
    private adminService: AdminService,
    private router: Router,
    private activedRoute: ActivatedRoute,
    private formBuilder: FormBuilder,
    private sharedService: SharedService,
    private blogService: BlogsService
  ) {}

  ngOnInit(): void {
    const id = this.activedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.addNew = false;
      this.blogService.editBlogTag(id).subscribe({
        next: (response: EditTag) => {
          this.formInitialized = true;
          this.initializeForm(response);
        },
      });
    } else {
      this.initializeForm(undefined);
    }
  }

  initializeForm(tag: EditTag | undefined) {
    if (tag) {
      this.tagForm = this.formBuilder.group({
        Id: [tag.id],
        Name: [tag.name, Validators.required],
        displayName: [tag.displayName, Validators.required],
      });
    } else {
      this.tagForm = this.formBuilder.group({
        Id: [''],
        Name: ['', Validators.required],
        displayName: ['', Validators.required],
      });
    }
    this.formInitialized = true;
    console.log(this.tagForm.get('Id')?.value);
  }

  submit() {
    this.submitted = true;
    this.errorMessages = [];
    if (this.tagForm.valid) {
      // let formValue! : EditTag;
      // formValue.id = this.tagForm.get('id')?.value;
      // formValue.name = this.tagForm.get('name')?.value;
      // formValue.displayName = this.tagForm.get('displayName')?.value;
      this.blogService.updateBlogTag(this.tagForm.value).subscribe({
        next: (response: any) => {
          this.sharedService.showNotification(
            true,
            response.value.title,
            response.value.message
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
