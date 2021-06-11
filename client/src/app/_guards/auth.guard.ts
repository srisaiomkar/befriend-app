import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private accountService : AccountService, private toastrService : ToastrService) {}

  canActivate():Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map((user : User) => {
        if(user) {
          return true;
        }
        this.toastrService.error('Sorry, you are not authorized to view this page');
        return false;
      })
    )
  }
  
}
