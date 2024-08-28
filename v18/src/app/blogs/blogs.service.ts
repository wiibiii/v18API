import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared/shared.service';
import { environment } from '../../environments/environment';
import { Home } from '../shared/models/blogs/home';
import { Tag, EditTag, Tags } from '../shared/models/blogs/tag';
import { AddBlogPostRequest } from '../shared/models/blogs/addBlogPostRequest';
import { BlogPost } from '../shared/models/blogs/blogPost';
import { BlogPostLike } from '../shared/models/blogs/blogPostLike';
import { BlogPostComment } from '../shared/models/blogs/comment';

@Injectable({
  providedIn: 'root',
})
export class BlogsService {
  constructor(
    private http: HttpClient,
    private router: Router,
    private sharedService: SharedService
  ) {}

  getAllBlogs(searchQuery: string, pageNUmber: number = 1) {
    let params = new HttpParams();
    params = params.append('pageNUmber', pageNUmber);
    params = params.append('searchQuery', searchQuery); //tag
    return this.http.get<Home | undefined>(
      `${environment.appUrl}bloghome/getblogs`,
      { params: params }
    );
  }

  getAllBlogsPaginated(
    searchQuery: string,
    sortBy: string,
    sortDirection: string,
    pageSize: number = 3,
    pageNUmber: number = 1
  ) {
    let params = new HttpParams();
    params = params.append('searchQuery', searchQuery);
    params = params.append('sortBy', sortBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('pageSize', pageSize);
    params = params.append('pageNUmber', pageNUmber);

    return this.http.get<BlogPost[]>(
      `${environment.appUrl}adminblogpost/get-all-blogs-paginated`,
      { params: params }
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

  editBlog(id: string) {
    let params = new HttpParams();

    params = params.append('id', id);

    return this.http.get<BlogPost>(`${environment.appUrl}adminblogpost/edit`, {
      params: params,
    });
  }

  addBlogPost(model: AddBlogPostRequest) {
    return this.http.post(
      `${environment.appUrl}adminblogpost/admin-add-blog`,
      model
    );
  }

  uploadImage(data: File) {
    const formData: FormData = new FormData();
    let headers: HttpHeaders = new HttpHeaders();
    formData.append('file', data, data.name);
    return this.http.post<string>(
      `${environment.appUrl}adminblogpost/images`,
      formData,
      {}
    );
  }

  getBlogPostByUrlHandle(urlHande: string) {
    let params = new HttpParams();

    params = params.append('urlHandle', urlHande);

    return this.http.get<BlogPost>(
      `${environment.appUrl}blogs/get-blog-by-urlhandle`,
      {
        params: params,
      }
    );
  }

  addLikeToBlogpost(blogpostid: string, userId: string) {
    let params = new HttpParams();

    params = params.append('BlogPostId', blogpostid);
    params = params.append('UserId', userId);

    return this.http.post<BlogPostLike>(
      `${environment.appUrl}BlogPostLike/add-like-request`,
      { blogpostid: blogpostid, userId: userId }
    );
  }

  addCommentToBlogpost(data: any) {
    // let params = new HttpParams();

    // params = params.append('Id', blogpostid);
    // params = params.append('commentDescription', commentDescription);
    // params = params.append('urlHande', urlHande);
    // const httpOptions = {
    //   headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    // };
    return this.http.post<BlogPostComment>(
      `${environment.appUrl}blogs/add-comment`,
      data
      // httpOptions
    );
  }
}
