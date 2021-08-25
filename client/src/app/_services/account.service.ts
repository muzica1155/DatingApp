import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators';// this works just like map function in javascript
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

///Angular services is a singleton when we inject int into a component & its initialized it will stay initialized until our app is disposed of the user closes the browser. for instance or they move away from our applicaition at that point our services is destroyed
//but if they on our applications then this account service will stay initiated through the lifetime that the app is around 

@Injectable({//services are injectable and are singleton// data we store inside the service doesn't get destroyed until our app is closed down
  //Components are different when we move from component to component in angulare they r destroyed as soon as they're not inuse, where as a service is a singleton& we typically 
  //use serives for making our http request 
  providedIn: 'root'
})
export class AccountService {
  //when we want to set this property to smthing we use equals, if we wanted to make it a type of smthing we would use a colon 
  //but if we just want to set this to smthing, we're going to use the equals to assign the Url
  // baseUrl = 'https://localhost:5001/api/'; // we do not hard coded sting in  our application //take advantage of environment files
 //store values in <user> store value 1 it means the size of our buffer 
 //just the user object for the current user, so we only need one of them & this is either gonna we null 
 //or its going to be current user object 
  baseUrl = environment.apiUrl;
  // .apiUrl;//
  private currentUserSource = new ReplaySubject<User>(1); //replaySubject is kind of like a buffer object is going to store the values inside here & any time a subscriber subscribes to less obseervable, its going to omit the last value inside it or however many values inside it that we want to admit 
  // & thew way that we r going to do this we r going to declare this as a private property & we r 
        //going to call it current user source & WE R GOING TO set this to
        // type of observablenow this is kind of a special type of observable called as replaySubject
    // new ReplaySubject<User>(1); // reason we set this up as an observable is os that this can be observed by other components or other classes in our application when whenver smthing subscribes to this, then it's going to be notified id anyhting changes 
    //<User>(1);//(1)//god is able to subscribe to this when we use the god it automatically subscribe to any observable we dont need to specifically subscribe to that current user observable in here bcoz the rootguard is automatically going to do this when we try & access that particular property
        // constructor(private http: HttpClient) { }//inject the http client into our account service 
  currentUser$ = this.currentUserSource.asObservable(); //this is currentUser$  going to be observable we give the dollar sign  at the end & then we say that the current user dollatr is equal to this 
  // currentUser$//we r going to have access to that property inside here & this is going to aloow us to set the user's image in navBAr
  constructor(private http: HttpClient) {}
   login(model: any)//method created logiis going to receive our credentials from the login form from our appbar 
   {
     // model contain our username & password that we send up to the server
     ///do smthig with this data inside the http post before we r subscribed to it nothing going to happen still even if we use the pipe & use some functionality from our RxJS 
     //so first of all, what we r going to do is use pipe method & we open some parentheses 
     //& anything that we put inside this pipe now is going to be in our RxJS operator
     // want to use map fucntions from RxJS which normally comes in automatically  

     return this.http.post(this.baseUrl + 'account/login', model).pipe(
       map((response: User) => { // curly brackets to add statements
        const user = response; // we r going to create unobservable to store our user in
        if (user) {
          //instead of two line we can set this too 1 line
          this.setCurrentUser(user);//& i reserve the right to come & change my mind about this 
      //     localStorage.setItem('user', JSON.stringify(user)); //this item a key of user, & then we r going to take the object we get back & stringify it 
      //   //now when we subscribe or when we log in because we r subscribing in our nav component then this function is going to run & it's going to populate our user inside local storage in the browser
      //  this.currentUserSource.next(user); //we r going to set our current user or the obsevable to replace subject & set this to the current user  we get back from our API
      }
       })
       //what we wanna do with response 
       //we want to get the user from the response 
     )
     //this is the very basic structure of a service  
   }
   register(model: any){ 
     //when a user registers we r going to consider them logged in to our app
     return this.http.post(this.baseUrl + 'account/register', model).pipe( // if we needed our user objects in the register component then what we would hav eto do inside this function is return from this 
       map((user: User) => { //we gonna map our user//map level wha twe ar getting back that user is a type of user 
        //if u r not sure or didn't have type for this at he moment always use any to get around any problem 
        //but we do hav user object we will specify or over user type for this 
        if (user) {//user object a type & will say & we need to do this at the map
          
          // localStorage.setItem('user', JSON.stringify(user));
          this.setCurrentUser(user);// when we get our user back from the API this is going to include that photo URl as well
          // this.currentUserSource.next(user);// problem we haven't given our user object to type & we have given it type to our current user source // not able ot set as object 
        // this.currentUserSource.next(user);//instead of using that method her we can kjust say this set current user & we can pass in the user 
        // we can do same for the login 
        }
        // return user;//we dont need the user just as an example gonna return the user to be clear what we r doing from our functon
        // just to be clear what we saw toreturn user from this 
      })
     )
     //'account/register', model)// passup model that we receive in our promises here 
   }

   setCurrentUser(user: User)// add helper method her called set current user
    {// shifted the local storage method to setCurrentUser from if (user)//
      localStorage.setItem('user', JSON.stringify(user));// lets tidy this up instead of using or havinf local storage in multiple places where we set the item
      this.currentUserSource.next(user);
    }
   logout() {//we want to do is be able to persist our in here so in our roots app component
     localStorage.removeItem('user');
     this.currentUserSource.next(null);
   }
  }
