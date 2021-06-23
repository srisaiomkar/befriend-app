import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild("editForm") editForm : NgForm;
  user : User;
  member : Member;

  constructor(private accountService : AccountService, private memberService : MemberService,
    private toastrService : ToastrService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=> this.user = user);
    this.memberService.getMember(this.user.username).subscribe(member =>this.member = member);
  }

  editMember(){
    console.log(this.member);
    this.toastrService.success("Profile updated successfully");
  }

}
