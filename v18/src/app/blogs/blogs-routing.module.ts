import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BloghomeComponent } from './bloghome/bloghome.component';
import { BlogsIndexComponent } from './blogs-index/blogs-index.component';

const routes: Routes = [
  { path: 'home', component: BloghomeComponent },
  { path: 'index/:urlHandle', component: BlogsIndexComponent },
];

@NgModule({
  declarations: [],
  imports: [
    //CommonModule
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})
export class BlogsRoutingModule {}
