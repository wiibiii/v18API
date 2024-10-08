import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MemberView } from '../shared/models/admin/memberView';
import { environment } from '../../environments/environment';
import { MemberAddEdit } from '../shared/models/admin/memberAddEdit';
import { EditTag, Tag } from '../shared/models/blogs/tag';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}

  getMembers() {
    return this.http.get<MemberView[]>(
      `${environment.appUrl}admin/get-members`
    );
  }

  getMember(id: string) {
    return this.http.get<MemberAddEdit>(
      `${environment.appUrl}admin/get-member/${id}`,
      {
        withCredentials: true,
      }
    );
  }

  getApplicationRoles() {
    return this.http.get<string[]>(
      `${environment.appUrl}admin/get-application-roles`
    );
  }

  addEditMember(model: MemberAddEdit) {
    return this.http.post(`${environment.appUrl}admin/add-edit-member`, model, {
      withCredentials: true,
    });
  }

  lockMember(id: string) {
    return this.http.put(`${environment.appUrl}admin/lock-member/${id}`, {});
  }

  unlockMember(id: string) {
    return this.http.put(`${environment.appUrl}admin/unlock-member/${id}`, {});
  }

  deleteMember(id: string) {
    return this.http.delete(
      `${environment.appUrl}admin/delete-member/${id}`,
      {}
    );
  }
}
