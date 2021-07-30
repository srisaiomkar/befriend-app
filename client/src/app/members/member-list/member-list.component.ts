import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  userParams : UserParams;
  members : Member[];
  pagination : Pagination;
  user : User;
  genderList = [{value:'male',display:'Men'},{value:'female',display:'Women'}];
  sortTypes =[{value:'lastActive', display :'Last Active Members'},
              {value:'accountCreated', display :'Newest Members'}];

  constructor(private memberService : MemberService,private accountService : AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user) 
    this.userParams = new UserParams(this.user);
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(){
    this.memberService.getMembers(this.userParams).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;
    });
  }

  pageChanged(event: any){
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }

  resetFilter(){
    this.userParams = new UserParams(this.user);
    this.loadMembers();
  }
}
