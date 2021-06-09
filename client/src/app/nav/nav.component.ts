import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  loggedIn : boolean = false;
  model : any = {};
  constructor(private accountService : AccountService) { }

  ngOnInit(){
    this.getCurrentUser();
  }

  login(){
    this.accountService.login(this.model).subscribe(response =>{
      console.log(response);
    },error =>{
      console.log(error);
    })
  }

  logout(){
    this.accountService.logout();
    this.loggedIn = false;
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe(user =>{
      this.loggedIn = !!user;
    },error =>{
      console.log(error);
    })
  }
}
