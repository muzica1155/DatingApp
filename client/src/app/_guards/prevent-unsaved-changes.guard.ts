import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {//CanDeactivate<unknown>// implement the can deactivate inteface
  canDeactivate( //method inside here 
    component: MemberEditComponent,):  | boolean  {//we can access the component that we r inside this case give this our member edit component bcoz this is going to give us access & give this our member edit component bcoz this going to give us access to our edit form 
    //bcoz we r going to need to check the status of the form inside here 
    // currentRoute: ActivatedRouteSnapshot,
    // currentState: RouterStateSnapshot,
    // nextState?: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean
    //  | UrlTree> | boolean | UrlTree {
      //component: MemberEditComponent,):// we got accces to our component 
    // we do not need router state // what we r going to return simple boolean 
    if (component.editForm.dirty) {//check the component state
       return confirm('Are you sure you want to continue ? Any unsaved changes will be lost ');
    // gives user yes or no options 
      }  return true;
  }
  
}
