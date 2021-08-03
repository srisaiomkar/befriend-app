import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  baseUrl = environment.apiUrl;

  constructor(private http : HttpClient) { }

  likeUser(username : string){
    return this.http.post(this.baseUrl + "likes/" + username,{});
  }

  getlikes(predicate : string){
    return this.http.get<Partial<Member[]>>(this.baseUrl + "likes?predicate=" + predicate);
  }
}
