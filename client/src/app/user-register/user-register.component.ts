import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  model : any = {};
  @Output() cancelRegisterEE = new EventEmitter(); 
  constructor(private accountService : AccountService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe();
    this.cancel();
  }

  cancel(){
    this.cancelRegisterEE.emit(false);
  }
}
