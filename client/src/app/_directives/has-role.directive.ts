 // its have directive decorator // the directives that we've been using so far in this course have been things like Asterix NGif or Asterix NgFor
  //we also used other directives like bs Radio & those rthe selectors we 've been using but in this case bcoz this is going to be a structural directives then the selectors look 
  //we r gonna t be using is *appHasRole & this is what we r gonna to be specifying when we make use of this directive 

  //how we do this ?
  //we need a constructor which we've got then inject several things 
  //remember the goal of this is to Hide or remove or show the admin link here but we r crating a directive so we can use it anywhere 
  //in our application not just on this specific link So we r gonna be removing or showing an element on the DOM depending on whether or not a user 
  //is in a role 

  //what we wanna take inside this directive //HasRoleDirective//when the user actually or when we actually use this//'[appHasRole]'//
    //so we gonna specify this selector *appHasRole// but what we want to pass to this //*appHasRole= is some parameters & the wya that we r gonna to pass
    //them we r gonna to say ask the user to pass us an array or more accurately inside quotes tey're gonna pass us an array of which roles the 
    //*appHasRole='["Admin"]'// the user needs to be in to access whatever we r using the directive on 
    //what we gonna do 
import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {//also what to do here is access the only initialization of lists as well implement the OnInit interface
  @Input() appHasRole: string[];//we r gonna take an input property so that we can get access to those parameters & we'll call this app has role 
  
  user: User;

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, 
    private accountService: AccountService) //inside constructor we need to get access to our current user & we'll need to go & get this 
    {
      this.accountService.currentUser$.pipe(take(1)).subscribe(user => 
      { // need a property for our user
        this.user = user;// so we have access to our user now 
      })
     }

  ngOnInit(): void {
      //clear view if no roles
       //the user doesn't have any roles then wer just gonna to clear the container 
      if (!this.user?.roles || this.user == null) // if they 've got no role the it's gonna to be optional ther should be no user that have this But u have to check this
    //then they r not authenticated effectively so we wanna say this .
    {
      this.viewContainerRef.clear();
      return;// & then we return from our Directive 
    }

      if (this.user?.roles.some(r => this.appHasRole.includes(r)))// add another condition & say //some thi is just determines whether a specified callback function returns true for any element of an array
      // what we gonna do is apply a callback function on each elements inside the user roles & wil say some (r =>) & we r gonna check against 
      //what we r inputting here  // r represent the role 
      {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
        //if the user does have a role & thats opened up our nav component SO if the user has a role that's in that list 
        //then we r gonna crate this embedded view & use this as it template reference
        //<li class="nav-item"> <a class="nav-link" routerLink='/admin' routerLinkActive='active'>Admin</a></li>
       
      } //SO We gonna add the admin link if they r in that role & if they r not in that role then just add else conditionsl
      else {
        this.viewContainerRef.clear();// then clear it so they do not see the admin link 
      }
  }

}
