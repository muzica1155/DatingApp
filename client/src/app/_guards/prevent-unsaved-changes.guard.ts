import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { AccountService } from '../_services/account.service';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> { //CanDeactivate<unknown>// implement the can deactivate inteface
     constructor(private confirmService: ConfirmService)// need to bring our model service 
     {

     }
  canDeactivate( //method inside here 
    //changes after optimization message 
    component: MemberEditComponent):  Observable<boolean> | boolean {
    // component: MemberEditComponent,):  | boolean  {//we can access the component that we r inside this case give this our member edit component bcoz this is going to give us access & give this our member edit component bcoz this going to give us access to our edit form 
    //changes after optimization message 

    //bcoz we r going to need to check the status of the form inside here 
    // currentRoute: ActivatedRouteSnapshot,
    // currentState: RouterStateSnapshot,
    // nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean
    //  | UrlTree> | boolean | UrlTree {
      //component: MemberEditComponent,):// we got accces to our component 
    // we do not need router state // what we r going to return simple boolean 
    if (component.editForm.dirty) {//check the component state
      //  return confirm('Are you sure you want to continue ? Any unsaved changes will be lost ');//instad of returnig javascript confirm we can replace this & sat returb
       return this.confirmService.confirm()// we r not returning a boolean now this is a observable so we 
      // gives user yes or no options 
      }  return true;// now we still want to return a boolean from this bcoz if the form is not dirty then we just wanna return true 
      // we dont need a boolean of trees for this simply wana return true So that they can move on to a differnt component 
      // so what we'll also do & the technical term for this is a union type will also return a boolean So error goes away & we can either return an 
      //observable or a boolean from this particular method //now bcoz we r inside a route guard here hten even though we r returning 
      //unobservable from our confirm method bcoz we r inside I regard this is automatically gonna subscribe for us take a look see it wroks 
      //what we can also do with this bcoz we didn't need to subscribe to this bcoz we r inside a route guard & the guard is automatically
      //going to subscribe for us  SoHOW DO WE USE THIS IN ANOTHER PLACE ?// LETS GO TO MESSAGES
      //
  }
  
}
