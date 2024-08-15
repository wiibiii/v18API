import { BloghomeComponent } from '../../../blogs/bloghome/bloghome.component';
import { BlogPostLike } from './blogPostLike';
import { Tag } from './tag';

export interface BlogPost {
  id: string;
  heading: string;
  pageTitle: string;
  content: string;
  shortDescription: string;
  featuredImageUrl: string;
  urlHandle: string;
  publishedDate: Date;
  author: string;
  visible: boolean;
  tags: string;
  // likes: BlogPostLike[];
  // comment: BloghomeComponent[];
}
