export interface Pagination{
    pageNumber : number;
    itemsPerPage : number;
    totalPages : number;
    totalItems : number;
}

export class PaginatedResult<T>{
    result : T;
    pagination : Pagination;
}