import { Injectable } from '@angular/core';
import {
   Resolve,
  
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Injectable // MemberDetailedResolver// bcozthi is not the component then we do need to add the injectable operator onto this 
({
  providedIn: 'root'
}) // Resolvers instantiated in the same way as services really & what we r going to do is inside this class
export class MemberDetailedResolver implements Resolve<Member> {

   constructor(private memberService: MembersService)// give a constructor
   // earlieri mentioned the reason y we couldn't use the navigation extras just to make life easier forourselves go back to member service 
   //we did work on to go & get individual member out of our cache // another option for this would been to use navigation extras but surprisely
   //inside a route resolver when we activate a route supriseingly there's no way to access navigation extras inside this & that's the reason we did the extra
   // work earlier on to go & get member from our cache so inside the member details was over we r gonna go & get our member inside here
   //we do not need , state: RouterStateSnapshot// & we gonna return from this is an observable of member remove other things that we r not reurning 
   {

   } 
   
  // resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Member> {
    resolve(route: ActivatedRouteSnapshot): Observable<Member> {
     return this.memberService.getMember(route.paramMap.get('username'));// inside here we r gonna return 
     // we dont need to subscribe inside resolvers The router is gonna take care of this for us all we need to specify the method in 
   //our service that's gonna to return observable & the router is gonna subscribe automatically for us & deal with the subscription as well
     //& we can use these for anyting we have a reason to use this now but y can use this to go & get any type of data u want if u want 
     //to get ur data before u constructs ur template the rest of the app we've just been using conditionals & ngIf do we have this or 
     //do we not? & when we have it then angular is gonna display the content after it received what we have this is an alternative way
     //of doing that now head over our app rooting module bcoz we r going to need to do smthing inside the root for the member details 
     //component now
  }
}
