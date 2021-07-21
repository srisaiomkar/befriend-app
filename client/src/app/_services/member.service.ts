import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';


@Injectable({
  providedIn: 'root'
})
export class MemberService {
  members: Member[] = [];
  paginationResult : PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  baseUrl : string = environment.apiUrl;
  constructor(private http : HttpClient) { }

  getMembers(pageNumber?: number, itemsPerPage?: number){
    console.log(pageNumber);
    console.log(itemsPerPage);
    let params = new HttpParams();

    // we need to convert params to string because query string is a string
    params = params.append('pageNumber',pageNumber.toString());
    params = params.append('itemsPerPage',itemsPerPage.toString());
    console.log(params);
    // observe response returns the entire http response(http headers and body)
    return this.http.get<Member[]>(this.baseUrl + 'users',{observe:'response',params}).pipe(
      map(response =>{
        this.paginationResult.result = response.body;
        if(response.headers.get('Pagination') !== null){
          this.paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginationResult;
      })
    );
  }

  getMember(username: string){
    const member = this.members.find((member) => member.userName === username);
    if(member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member : Member){
    return this.http.put(this.baseUrl + 'users/',member).pipe(
      map(() => {
        var index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMain(id : number){
    return this.http.put(this.baseUrl + 'users/main-photo/' + id,{});
  }

  deletePhoto(id : number){
    return this.http.delete(this.baseUrl + 'users/photo/' + id);
  }
}
