import { PaginationParams } from "./paginationParams";

export class LikesParams extends PaginationParams{
    predicate : string;

    constructor(predicate : string){
        super();
        this.predicate = predicate;
    }
}