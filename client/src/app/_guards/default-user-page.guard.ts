import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class DefaultUserPageGuard implements CanActivate {
  user : User;
  constructor(private accountService : AccountService, private router : Router){
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user )
  }

  canActivate() : boolean{
    if(this.user){
      this.router.navigateByUrl('/members');
      return false;
    }
    return true;
  }
  
}
