import { Component, OnInit } from '@angular/core';
// import { UserInfo } from 'os';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'] //angulat provides two way binding we can bind data and properties in our component.and display them in our templete
  //we can take data which is entered in the form and update smthing inside our component known as two ways binding


})
export class NavComponent implements OnInit {
  ///create a class property to store 
  model: any = {}
  // loggedIn: boolean;
  // currentUser$: Observable<User>;
  // constructor(private accountService: AccountService)// if we want to access inside our templete we have to make it public 
  constructor(public accountService: AccountService, private router: Router,
     private toastr: ToastrService)
  //WE CAN INJECT OUR ROUTER INTO OUR COMPONENT INSIDE THE constractor JUST LIKE WE DO FOR SERVICES
  { }

  ngOnInit(): void { // do smthing bcoz we r getting the current user call th function to go & gets the current user from the account service 
    //  this.getCurrentUser();
    // this.currentUser$ = this.accountService.currentUser$;

  }
  login(){
    // console.log(this.model);
    this.accountService.login(this.model).subscribe(response => 
      {
        this.router.navigateByUrl('/members');
        // console.log(response);
        // this.loggedIn = true;

    }, error => {
      console.log(error);
      this.toastr.error(error.error);//
     // this.toastr.error(error.error);///we should hace http response itself is contained inside the error but the error message is contained inside an error property 
    })//(this.model) an observable oject this may casue a slight issue, So unobservanle is lazy it doesn't do anything until we subscribe to the observable
    //so we have to subscribe & then we r going to get a response back fro our server than we gonna get response back from server
    // now when we log in we r going to get user DTO return to us 
  }
  logout() {
    this.accountService.logout();
    //interrogate our account service & take a look inside that current user
    // this.loggedIn = false;
    this.router.navigateByUrl('/')
  }
  // getCurrentUser() {  
  //   //.currentUser$.subscribe// we aRE subscribing TO CURRENT USER BUT CURRENT USER IS NOT AN HTTP REQUEST THIS NEVER COMPLETE
  //   this.accountService.currentUser$.subscribe(user => { //our user is even null or its a user object if we use // !! // we r saying if user is null then its false if user is smthing then true 
  //     //we are subscribing  we can also take of error in any points 
  //     this.loggedIn = !!user; // !! //turn our oject into a boolean
  //   }, error => {
  //     console.log(error);
  //   })
  // }

}
