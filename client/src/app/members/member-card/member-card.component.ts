import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { LikesService } from 'src/app/_services/likes.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member : Member;
  constructor(private likesService : LikesService, private toastrService : ToastrService) { }

  ngOnInit(): void {
  }

  likeUser(username : string){
    this.likesService.likeUser(username).subscribe(()=>{
      this.toastrService.success("You have liked " + username);
    })
  }
}
