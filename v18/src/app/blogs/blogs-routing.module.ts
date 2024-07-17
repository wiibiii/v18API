import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BloghomeComponent } from './bloghome/bloghome.component';

const routes: Routes = [{ path: 'home', component: BloghomeComponent }];

@NgModule({
  declarations: [],
  imports: [
    //CommonModule
    RouterModule.forChild(routes),
  ],
  exports: [RouterModule],
})
export class BlogsRoutingModule {}
