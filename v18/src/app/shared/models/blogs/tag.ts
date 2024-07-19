import { BlogPost } from './blogPost';

export interface Tag {
  id: string;
  name: string;
  displayName: string;
  blogPosts: BlogPost[];
}

export interface EditTag {
  id: string;
  name: string;
  displayName: string;
}
