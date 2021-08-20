import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyRequestCount = 0;//add properties //we could have multiple requests going on at a time, so we r going to increment this counter & decrement This counts each time we make a request & a request completes

  constructor(private spinnerService: NgxSpinnerService) { }

   busy() {
     this.busyRequestCount++; //to INcremented 
     this.spinnerService.show(undefined, {
      type: 'line-scale-party',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333'
    });//add some configuration properties false// different option 50 spinner types 
    }// methods

    idle()//when we r ideal
    { 
      this.busyRequestCount--; //
      if(this.busyRequestCount <= 0) 
      {
        this.busyRequestCount = 0;
        //safty mechanism 
        this.spinnerService.hide();//just in case our busy  service gets  a little bit confused about number of request have been completed
         //if it is less than 0 then we r just going to set it to zero  Otherwise if it does manage to get itself into a minus 1 then next time we make a request its 
      }
    }
}
// in angular we got a facility to intercept requests as they go in & they go out & we can use an interceptor to take care of this 