import { BlogPost } from './blogPost';
import { Tag } from './tag';

export interface Home {
  blogPosts: BlogPost[];
  tags: Tag[];

  blog: BlogPost[];
}
