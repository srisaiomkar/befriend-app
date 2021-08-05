import { PaginationParams } from "./paginationParams";
import { User } from "./user";

export class UserParams extends PaginationParams{
    gender: string;
    minAge: number = 18;
    maxAge: number = 99;
    orderBy: string = 'lastActive';
    constructor(user :User){
        super();
        this.gender = user.gender === "male"? "female":"male";
    }
}