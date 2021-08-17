import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {} // what we want to do bcoz we r storing our token as part of our current user inside account service,
  //we'll do is we'll bring our account service and inject this into this particular constructor
  //we got our curretn user in our whe we login we set that particular property and current user is unobservable inside there
 // we need to do in order to use the token from inside there we r going to need to get the current
 //user outside of that observable 
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;//this should contain the contents of that current user or its going to be now 
    //const cant be used bcoz we dont have a current user yet we use let
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);//getting back the current user then we set our current user from the account service to this current user variable that we just created 
    if (currentUser) { // wanto clone the request & add our authentication header onto it 
    request = request.clone({
    setHeaders: {
      Authorization: `Bearer ${currentUser.token}` //beatix on this code to allow us not to need to allow us to concatenate directly inside the same string 
    }//'Bearer '//very important space bcoz nothing works authenitacations wise if u join your token with the word barer there has to be that space inside there 
    }) //check to see if we have a current user inside there 
    //we need to subscribe to go & get ehat inside the observable out of the observable ?
    //but dowe also need to unsubscribe fromthis ? just to make user pipe method take 1 from observables
    //and then we will subscribe
    //Authorization: 'Bearer ${currentUser.token}' //whAT this goona do is attach our token for every request when we r logged in and send that up with our request bcoz we clone this request here request = request.clone({//when wereturn from this return next.handle(request);
    //it we r logged in that going to receive our authorization header & send this up with our request 
    //pipe(take(1))//by using take & just by taking one thing from this obserable what we r doing here is we r saying that 
    //we want to complete after we r received one of these current users and this way we dont need to unsubscribe 
    //bcoz once unobservable had complete then we are effectively not subscribe to it anymore 
    //wehnever we r not sure if we need to unsubscribe from smthing then we can do at that pipe then take 1 in this case then we can go & subscribe & we kind of guarentee
    //we unsubscribe from that bcoz our interceotors r going to be initialized when we start our app and
    //bcoz they r part of our modeile & we add them to the providers & they r always going to be arounf of
    //until we close our application//thia techique continuously to ensure that anywhere else we use this we do take care of completing our subscription 
    }
    return next.handle(request);
  }
}
