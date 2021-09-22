import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private accountService: AccountService, private toastr: ToastrService)// need to give this constructor bcoz we r gonna need to access our account service & we'l say constructor 
  {
    //inside here we do similar what we do with our offguard this time we need to check the roles that current user is actually in So Once 
    //again we don't want deafault settign of thr admin.guard
  }
  canActivate(): Observable<boolean>  {
    //al we gonna return from here is a boolean  & we get a return on a Observable of a boolean here 
    return this.accountService.currentUser$.pipe // same as we did with the auth guard BUT this time we r gonna need to check what roles the user is in 
    //So we need to use the map so that we can use a callback function on our user & the user we get back from our current user 
    //when we do this & we r gonna check to see if the user 
    (
      map(user => {
        if (user.roles.includes('Admin') || user.roles.includes('Moderator')) //look for admin & we als want ot allow for moderators so we r gonna to add Or condition &
        //yes spelling is important as per normal wen we r dealing with strings & if that is the case we r gonna return 
          {
            return true;
          }
          //underneath this if statement IF that is not the case then we r gonna to say this 
          this.toastr.error('You cannot enter this area');
          return false;    
      })
      
    )  
  }
  
}
