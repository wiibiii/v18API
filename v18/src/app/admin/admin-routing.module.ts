import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';

import { AddEditMemberComponent } from './add-edit-member/add-edit-member.component';
import { AdminGuard } from '../shared/guards/admin.guard';
import { ListTagsComponent } from './blogs/tags/list-tags/list-tags.component';
import { EditTagsComponent } from './blogs/tags/edit-tags/edit-tags.component';
import { AddTagsComponent } from './blogs/tags/add-tags/add-tags.component';
import { BlogPostsComponent } from './blogs/blog-posts/blog-posts.component';
import { ListBlogpostsComponent } from './blogs/list-blogposts/list-blogposts.component';

const routes: Routes = [
  //{ path: '', component: AdminComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AdminGuard],
    children: [
      { path: '', component: AdminComponent },
      //path for creating a new member
      { path: 'add-edit-member', component: AddEditMemberComponent },
      //path for editing an existing member
      { path: 'add-edit-member/:id', component: AddEditMemberComponent },
      { path: 'admin-tags', component: ListTagsComponent },
      { path: 'admin-edit-tags', component: EditTagsComponent },
      { path: 'admin-edit-tags/:id', component: EditTagsComponent },
      { path: 'admin-add-tag', component: AddTagsComponent },
      { path: 'admin-blogpost', component: ListBlogpostsComponent },
      { path: 'admin-edit-blogpost', component: BlogPostsComponent },
      { path: 'admin-edit-blogpost/:id', component: BlogPostsComponent },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
