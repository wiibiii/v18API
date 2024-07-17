import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from '../shared/shared.service';
import { environment } from '../../environments/environment';
import { Home } from '../shared/models/blogs/home';

@Injectable({
  providedIn: 'root',
})
export class BlogsService {
  constructor(
    private _http: HttpClient,
    private router: Router,
    private sharedService: SharedService
  ) {}

  getAllBlogs() {
    return this._http.get<Home | undefined>(
      `${environment.appUrl}bloghome/getblogs`
    );
  }
}
