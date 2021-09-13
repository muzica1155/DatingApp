import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
 import { Observable, pipe, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
//the goal of using the arrow interceptor & doing work up front to handle all of these is to make this as easy as possible in our app so we dont really need to worry about errors
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) //add route the reason ;- that for certain type of errors we r going to want to redirect the user to an error page 
  {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> { //the request what we get back from thi si unobservable, so in order to do smthing eith this like any other observable we r going to need to use the pipe method to do whatever functionality we want 
    return next.handle(request).pipe
    ( catchError(error => {
      if (error) {  // going to do switch statements so we can switch depending on what status smthing is 
        switch (error.status) {
          case 400:// ticky on e 400 there are different types of 400 errors
                if (error.error.errors)// check to see if there is any if there's an error object that has an object called errors inside it  
            { 
              const modalStateErrors = [];// add a variable for model status errors //this is what leno as in ASp dot net full of validations errors
              for ( const key in error.error.errors ) // loop over each key in array object 
              { 
                if (error.error.errors[key]) // we r not using typescript for type safety
                {
                  modalStateErrors.push(error.error.errors[key]);// idea of this is to flatten the array of errors that we get back from our validation responses & push them int an array 
                }
              }
               throw modalStateErrors.flat();// throw the modelStateError back to the component reason;- bcoz if we take our registration form what we have to do in register link if we get some error bak from the api it not really suitable to display a toast but ok is to display list of validation errors underneath the form 
            }//if we have an array of arrays then we can flatten them using the flat method so we take our 
            // why cant we use it 
            // else // we'll check the other situation if its just a normal 400 error then underneath thi if statement if toast 
            else if (typeof(error.error) === 'object') // only if the error object is an object 
            // to check to see if smthing is an object is we can specify and else if in this case we say typeOf & an error
            //(error.error) === 'object')//thi is how we can check to see if smthing is an object bcoz the othe case that we'll just put 
            //inside & else 
            {
              this.toastr.error(error.statusText, error.status);                     
            }
            else
            {
              this.toastr.error(error.error, error.status);//we know that this at this stage this is going to be a string we do here
               //what they should mean is that our error behave correctlynow
            }
            break;
            case 401: 
              this.toastr.error(error.statusText, error.status);
              break;
              case 404: ///this.router.// bcoz we r going to redirect them to a not found page &we r going to add later a not found componen that we can redirect them to & then we'll break the
                this.router.navigateByUrl('/not-found');
                break;
                case 500://again we r going to redirect them to a server error page but what we wnat to do is also get the details of the error that we r going to get returned from the API 
                // we can do this use the feature of the router wher we can pass it some states use naviagaion extras to do this 
                const navigationExtras: NavigationExtras = {state: {error: error.error}}; // which is gonna be exception that we get back from API & 
                //const navigationExtras: we r adding the error to the router state through this navigation extras //
                //what to do when that component loads is ge hold of that error so tha we can do smthing inside the page after we've redirected the user to this particular route
                this.router.navigateByUrl('/server-error', navigationExtras);//pass // navigationExtras);// as the states
                break;
                default:
                  this.toastr.error('Something unexpected went wrong ');
                  console.log(error);// this will give us oppertunity to go & tweak our interceptor bcoz we need to add new case to take care of that 
            break;
        }
      }
      return throwError(error); // this should remove the particaular error form this //(error => {
    }) //rxjs operator//(error)//this going to be http response that we saw in our console logs when clicked on & inside this error & check to see if we have an error 

    );//inercept method //what we can do either intercept the request that goes out or the response that comes back in the next wher we handle the response
  }// we return next handle & then the request itself want to do is catch any errors from this request 
}
