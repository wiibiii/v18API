import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared/shared.service';
import { environment } from '../../environments/environment';
import { Home } from '../shared/models/blogs/home';
import { Tag, EditTag, Tags } from '../shared/models/blogs/tag';
import { AddBlogPostRequest } from '../shared/models/blogs/addBlogPostRequest';

@Injectable({
  providedIn: 'root',
})
export class BlogsService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private sharedService: SharedService
  ) {}

  getAllBlogs() {
    return this.http.get<Home | undefined>(
      `${environment.appUrl}bloghome/getblogs`
    );
  }

  getAllBlogTagsPaginated(
    searchQuery: string,
    sortBy: string,
    sortDirection: string,
    pageSize: number = 3,
    pageNUmber: number = 1
  ) {
    // searchQuery=${searchQuery}&sortBy=${sortBy}&sortDirection=${sortDirection}&
    let params = new HttpParams();
    params = params.append('searchQuery', searchQuery);
    params = params.append('sortBy', sortBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('pageSize', pageSize);
    params = params.append('pageNUmber', pageNUmber);
    return this.http.get<Tags[]>(
      `${environment.appUrl}admintags/get-all-tag-paginated`,
      {
        params: params,
      }
    );
  }

  getAllBlogTags() {
    return this.http.get<Tag[]>(
      `${environment.appUrl}admintags/get-all-blog-tags`
    );
  }

  editBlogTag(id: string) {
    let params = new HttpParams();

    params = params.append('id', id);

    return this.http.get<EditTag>(`${environment.appUrl}admintags/edit`, {
      params: params,
    });
  }

  updateBlogTag(model: EditTag) {
    return this.http.post(`${environment.appUrl}admintags/edit`, model);
  }

  deleteBlogTag(id: string) {
    return this.http.delete(
      `${environment.appUrl}admintags/delete-tag/${id}`,
      {}
    );
  }

  editBlog(id: string) {}

  addBlogPost(model: AddBlogPostRequest) {
    return this.http.post(
      `${environment.appUrl}adminblogpost/admin-add-blog`,
      model
    );
  }
}
