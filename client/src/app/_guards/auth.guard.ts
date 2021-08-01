import { Injectable } from '@angular/core';
import { CanActivate} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';


@Injectable({
  providedIn: 'root'
}) //this is decorated with the injectable we r not going to inject this into any of our components but we dot need to do anyting with ir 
export class AuthGuard implements CanActivate {//Authguard which is a class which implements the can acrivate interface//we can have multiple of this CanActivate// & if any one of thr guards fails then the whole thing fails 
  constructor(private accountService: AccountService, private toastr: ToastrService) //
  {

  }
  canActivate(): Observable<boolean>  { //thi is a list of different things that we can return from this guard 
    // we want to return from this ^ is observables of a boolean 
    // route: ActivatedRouteSnapshot,
    // state: RouterStateSnapshot)//we get access to inside here is the route that is being activated & the current state of our router
      return this.accountService.currentUser$.pipe // bcoz our current user observable return a user or it would do if we r subscribing to it bcoz we r inside a router offguard,its going to handle the subscription use the pipe & then the map in this case 
      (
        map(user => {
         if (user) return true; //we going to check to see if we have a user// return true;//this is observable of true now which matches the return type for they can activate method 
          
         this.toastr.error('You shall not pass!');
         return false;
         //if u do not have a user we will return a toast 
          ///what u need to pic here is angular is gandalf & the unauthenticated user 
          //this is effectively going to do is protect our roots now or it will do once we confire out our routline configurations
        })

      )
  }
  
}
