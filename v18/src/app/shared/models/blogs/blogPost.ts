import { BloghomeComponent } from '../../../blogs/bloghome/bloghome.component';
import { BlogComment } from './blogComment';
import { BlogPostLike } from './blogPostLike';
import { BlogPostTags } from './blogPostTags';
import { Tag, Tags } from './tag';

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
  tags: BlogPostTags[];
  likes: BlogPostLike[];
  comments: BlogComment[];
  liked: boolean;
  totalLikes: number;
}
