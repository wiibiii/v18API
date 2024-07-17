import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BloghomeComponent } from './bloghome/bloghome.component';
import { BlogsRoutingModule } from './blogs-routing.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [BloghomeComponent],
  imports: [CommonModule, BlogsRoutingModule, SharedModule],
})
export class BlogsModule {}
