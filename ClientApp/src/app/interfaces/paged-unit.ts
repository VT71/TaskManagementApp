/**
 * Generic interface representing a paginated result set.
*/
export interface PagedUnit<T> {
    totalCount: number;
    items: T[];
}