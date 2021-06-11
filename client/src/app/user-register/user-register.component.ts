import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  model : any = {};
  @Output() cancelRegisterEE = new EventEmitter(); 
  constructor(private accountService : AccountService,
              private toastrService : ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(response =>{
      console.log(response);
    },error =>{
      console.log(error);
      this.toastrService.error(error.error);
    });
    this.cancel();
  }

  cancel(){
    this.cancelRegisterEE.emit(false);
  }
}
