import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminComponent } from './admin.component';
import { AddEditMemberComponent } from './add-edit-member/add-edit-member.component';
import { AdminRoutingModule } from './admin-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AddTagsComponent } from './blogs/tags/add-tags/add-tags.component';
import { EditTagsComponent } from './blogs/tags/edit-tags/edit-tags.component';
import { ListTagsComponent } from './blogs/tags/list-tags/list-tags.component';
import { BlogPostsComponent } from './blogs/blog-posts/blog-posts.component';
import { ListBlogpostsComponent } from './blogs/list-blogposts/list-blogposts.component';
import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';
import 'froala-editor/js/plugins.pkgd.min.js';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtInterceptor } from '../shared/interceptors/jwt.interceptor';
@NgModule({
  declarations: [
    AdminComponent,
    AddEditMemberComponent,
    AddTagsComponent,
    EditTagsComponent,
    ListTagsComponent,
    BlogPostsComponent,
    ListBlogpostsComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SharedModule,
    FroalaEditorModule.forRoot(),
    FroalaViewModule.forRoot(),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true,
    },
  ],
})
export class AdminModule {}
