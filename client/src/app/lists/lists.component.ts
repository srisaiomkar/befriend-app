import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { LikesService } from '../_services/likes.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members : Partial<Member[]>;
  predicate = 'liked';

  constructor(private likesService : LikesService) { }

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes(){
    this.likesService.getlikes(this.predicate).subscribe(response =>{
      this.members = response;
    });
  }

}
