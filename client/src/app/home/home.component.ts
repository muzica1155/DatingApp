import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  // users: any;// pass the users to home component

  // constructor(private http: HttpClient) { }
  constructor() { }
  ngOnInit(): void {//add it ot the app or component initialization calling the method 
    // this.getUsers();
  }
  registerToggle() {
    this.registerMode = !this.registerMode;
  }
  // getUsers() {
  //   this.http.get('https://localhost:5001/api/users').subscribe(users => this.users = users);
  //   //response that we r get back from the API we know in the response body is an array of users & we r basicaly setting less users property in our class to the users that we get BACK THE RESPONSE unlesss w 've got multiple statements we can do it this way & a single line if we want to 
  // }
  cancelRegisterMode(event: boolean ) //boolean in case bcoz that what emiiting from our child component 
  {
    this.registerMode = event;//when we set the cancel register mode to what that event is then that event should be false which then turns list registe into false & will stop displaying the register form 
  }

}
