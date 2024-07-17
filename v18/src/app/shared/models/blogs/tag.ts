import { BlogPost } from './blogPost';

export interface Tag {
  Id: string;
  Name: string;
  DisplayName: string;
  BlogPosts: BlogPost[];
}
