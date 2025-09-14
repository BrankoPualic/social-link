export class PagedResponse<T> {
  totalCount?: number;
  currentPage?: number;
  pageSize?: number;
  pageCount?: number;
  items?: T[];
}
