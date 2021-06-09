import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Befriend';
  users: any;
  constructor(private http : HttpClient,private accountService : AccountService){}

  ngOnInit() {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(){
    this.http.get("https://localhost:5001/api/users").subscribe(response =>{
      this.users = response;
    },error =>{
      console.log(error);
    })
  }

  setCurrentUser(){
    const user = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }
}
