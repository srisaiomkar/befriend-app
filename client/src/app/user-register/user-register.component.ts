import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {
  model : any = {};
  registerForm : FormGroup;
  @Output() cancelRegisterEE = new EventEmitter(); 
  constructor(private accountService : AccountService,
              private toastrService : ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      username : new FormControl('',Validators.required),
      password : new FormControl('',Validators.required),
      confirmPassword : new FormControl('',[Validators.required,this.matchWith('password')])
    });

    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchWith(s : string) : ValidatorFn{
    return (control : AbstractControl) =>{
      return control?.value === control?.parent?.controls[s]?.value? null : {isNotMatching : true}
    }
  }

  register(){
    console.log(this.registerForm.value);
    // this.accountService.register(this.model).subscribe(response =>{
    //   console.log(response);
    // },error =>{
    //   console.log(error);
    //   this.toastrService.error(error.error);
    // });
    // this.cancel();
  }

  cancel(){
    this.cancelRegisterEE.emit(false);
  }
}
