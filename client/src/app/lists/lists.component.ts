import { Component, OnInit } from '@angular/core';
import { LikesParams } from '../_models/likesParams';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { LikesService } from '../_services/likes.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members : Partial<Member[]>;
  likesParams = new LikesParams('liked');
  pagination : Pagination;

  constructor(private likesService : LikesService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.likesService.getlikes(this.likesParams).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;
    });
  }

  pageChanged(event : any){
    this.likesParams.pageNumber = event.page;
    this.loadLikes();
  }

}
