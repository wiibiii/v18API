export interface AddBlogPostRequest {
  heading: string;
  pageTitle: string;
  content: string;
  shortDescription: string;
  featuredImageUrl: string;
  urlHandle: string;
  publishedDate: string;
  author: string;
  visible: boolean;
  tags: string[];
  selectedTags: string[];
}
