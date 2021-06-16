import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toastrService: ToastrService, private router : Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error =>{
        if(error){
          switch(error.status){
            case 400:
              error.statusText = 'Bad Request';
              if(error.error.errors){ 
                var errorMessages =[];
                for(var key in error.error.errors){
                  if(error.error.errors[key])
                    errorMessages.push(error.error.errors[key]);
                }
                throw errorMessages.flat();
              }else{
                this.toastrService.error(error.statusText,error.status);
              }
              break;
            case 401:
              error.statusText = 'Unauthorized';
              this.toastrService.error(error.statusText,error.status);
              break;
            case 404:
              this.router.navigateByUrl('not-found');
              break;
            case 500:
              var navigationExtras :NavigationExtras = {state : {error : error.error}} 
              this.router.navigateByUrl('server-error',navigationExtras);
              break;
            default:
              this.toastrService.error("Something went wrong");
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
