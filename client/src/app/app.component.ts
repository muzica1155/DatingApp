import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {// angular come with life cycle events that take place our constructor lead us to initialize
  title = 'The Dating App';//we can pass data from our component to our view templete itself we do this wit interperlation 
  users: any;// typescript give us type safty unless we use this particular users properties: any ;
  // constructor(private http: HttpClient, private accountService: AccountService)// use app component to fetch the data and lend display on the page using dependence injection 
  constructor(private accountService: AccountService)//bring in our account service into our app components 
  
  { 


   }
  ngOnInit() {
    // this.getUsers();
    this.setCurrentUser();
  }
    
  //call this method setCurrentUser() when we initialize the this component
  setCurrentUser()//we create a new method & we call it current user user inside here inside the app componentuse the same name
      {
        const user: User = JSON.parse(localStorage.getItem('user'));  //add a varaible bring on use object add this to our import use JSON.parse() to get the object out of this stratified form into our user object here
        
        this.accountService.setCurrentUser(user);//
      }
  //   getUsers() //separate method
  // {
  //   this.http.get('https://localhost:5001/api/users').subscribe(response => {
  //     this.users = response;
  //   }, error => {
  //     console.log(error);
  //   }) // this.http service we have to use this service if we want to acces any property inside a class 
  //  ///observables they are lazy they dont do anything unless somebody subscribe to them observable can be streams of data
  //  //not in a case in http request //tell subscribe methods what to do next time
  //  //when data come back to us 
  // }
}
