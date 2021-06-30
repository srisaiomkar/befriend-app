import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {
  busyRequestCount = 0;
  constructor(private ngxSpinnerService : NgxSpinnerService) { }
  
  activate(){
    this.busyRequestCount++;
    this.ngxSpinnerService.show(undefined,{
      type : 'ball-spin-fade',
      bdColor: 'rgba(0, 0, 0, 0.8)',
      color : '#fff'
    });
  }

  deactivate(){
    if(this.busyRequestCount > 0) this.busyRequestCount--;
    if(this.busyRequestCount == 0){
      this.ngxSpinnerService.hide();
    }
  }

}
