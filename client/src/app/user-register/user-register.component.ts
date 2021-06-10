import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  model : any = {};
  @Output() cancelRegisterEE = new EventEmitter(); 
  constructor() { }

  ngOnInit(): void {
  }

  register(){
    console.log(this.model);
  }

  cancel(){
    this.cancelRegisterEE.emit(false);
  }
}
