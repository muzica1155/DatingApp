import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable, Observer } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  bsModelRef: BsModalRef;//give it a property based model worth of type based model 
  //inject the model service inside here 

  constructor(private modelService: BsModalService) //inject the model service inside here 
  { }

  confirm(title = 'Confirmation', 
  message = 'Are you sure you want do this', 
  btnOkText = 'Ok', 
  btnCancelText = 'Cancel') //single method //give them some property & default values that we can overwrite if we want to 
                  : Observable<boolean>//& WHAT WE WANT to return from this particular method & I'll just specify it here to be explicit
                  //& then our components or whatever it is that uses this particular confirm dialogue can then subscribe to get the result
            //out of it so we need to return observable from a subscription when we subscribe to smthing we get a subscription back but we 
            //need to return that as an observable i m gonna do to help for this create a private method
   {
     const config = // just other 1 we r gonna give it some configuration so we can say const config equals & then we r 
         {
           initialState: {
             title,//inital state is gonna be the title the message the button Ok text & a button 
             message,
             btnOkText,
             btnCancelText,
           }
         }
        //  this.bsModelRef = this.modelService.show('confirm', config);
        this.bsModelRef = this.modelService.show(ConfirmDialogComponent, config);
        //now we can show this particular model & bcoz we r in a service we'll be able to use this from anywhere
        //but do we get the results from this model ?
        //when a user clicks on yes or no we want to be able to get true or false returned fromthis & what we can use for this inside the bsmodelref 
        // this.bsModelRef.// there is a property called unhidden & unhide & these emit events when the modal behind the ref finishes hiding 
        //or state hiding & from here we can get the results in order to get the result we have a subscribe to it but that doesn't help us return what er need 
        // to from the service we need to do smting else 
      
         //the tricky part with this & the but hard to find info about is how do u got the info out of the confirmation model?
         // what we need is the confirmation dialogue to go inside here 

         return new  Observable<boolean>(this.getResult());//want to return 

   }//& our result is gonna to give us smthing that we can pass as an observable so that we can use this inside our components
   //lets go to the prevent unsaved changes guard 
     private getResult()
     {
       return (observer) => // we need to return an observer we r gonna observe smthing inside what we r getting here & we'll 
       //add a
       {
         const subscription = this.bsModelRef.onHidden.subscribe// as this is what we r gonna be getting from the modal 
         //onhidden // as soon as the modal closes then we r gonna to get smthing & what we need to do here is subscribe to this & we r gonna to be
         //subscribing to the result but we r not gonna get it passed in our next inside this subscription so
         (
           () => {
             observer.next(this.bsModelRef.content.result);//& we r setting up our own observable here that we r returning so we r giving it & observe a dot next 
              observer.complete();//& then we just specify an observer dont complete
            }
         );
         return {
                unsubscribe()// & we need to give it an unsubscribe method 
                {
                  subscription.unsubscribe();
                }
         }
       }
     }

}
