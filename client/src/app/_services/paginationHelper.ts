import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";

export function getPaginatedResult<T>(url,params,http : HttpClient){
    /* In JavaScript/Typescript, local variables of a function will not be destroyed after the 
    function returns if there is at least one reference on them.
    Here paginated result's reference will be returned which means it has a reference on it
    , so it will not be destroyed after the function call ends*/
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    // observe response returns the entire http response(http headers and body)
    return http.get<T>(url,{observe:'response',params}).pipe(
        map(response =>{
            paginatedResult.result = response.body;
            if(response.headers.get('Pagination') !== null){
              paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
            }
            return paginatedResult;
        })
    );
}