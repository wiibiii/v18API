import { BlogPost } from './blogPost';

export interface Tag {
  id: string;
  name: string;
  displayName: string;
  nlogPosts: BlogPost[];
}
