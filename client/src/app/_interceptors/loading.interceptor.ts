import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../_services/busy.service';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService: BusyService)
   {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
     this.busyService.busy();// when we about to send our request we r going to call
    
     return next.handle(request).pipe(// when the request comes back? its completed so we turn off our busy spinner use pipe 
      //production delay is commented 
        // delay(1000),//add a fake delay our application is way too fast
        finalize(() => {
          this.busyService.idle();//bcoz this a interceptor we need to go to app module and add
        })//takes some method 
      );  
    }
} 
