import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LikesParams } from '../_models/likesParams';
import { Member } from '../_models/member';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  baseUrl = environment.apiUrl;

  constructor(private http : HttpClient) { }

  likeUser(username : string){
    return this.http.post(this.baseUrl + "likes/" + username,{});
  }

  getlikes(likesParams : LikesParams ){
    let params = getPaginationHeaders(likesParams.pageNumber,likesParams.itemsPerPage);
    params = params.append('predicate',likesParams.predicate);

    return getPaginatedResult<Partial<Member[]>>(this.baseUrl+'likes',params,this.http);
  }
}
