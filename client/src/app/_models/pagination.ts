export interface Pagination{
    pageNumber : number;
    itemsPerPage : number;
}

export class PaginatedResult<T>{
    result : T;
    pagination : Pagination;
}