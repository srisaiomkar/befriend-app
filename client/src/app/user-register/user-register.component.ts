import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
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
  maxDate : Date;
  @Output() cancelRegisterEE = new EventEmitter(); 
  validationErrors : string[] = [];
  constructor(private accountService : AccountService,
              private toastrService : ToastrService,
              private fb : FormBuilder,
              private router : Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm(){
    this.registerForm = this.fb.group({
      gender : ['male',Validators.required],
      username : ['',Validators.required],
      nickname : ['',Validators.required],
      dateOfBirth : ['',Validators.required],
      city : ['',Validators.required],
      country : ['',Validators.required],
      password : ['',Validators.required],
      confirmPassword : ['',[Validators.required,this.matchWith('password')]]
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
    this.accountService.register(this.registerForm.value).subscribe(() =>{
      this.router.navigateByUrl('/members');
    },error =>{
      this.validationErrors = error;
    });
   }

  cancel(){
    this.cancelRegisterEE.emit(false);
  }
}
