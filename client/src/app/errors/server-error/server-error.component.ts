import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent implements OnInit {
  error: any; //add a class property for the area so we'll add a class property forthe area 

  constructor(private router: Router)//inject our router into this so we have got acces to the router state
  // ther only one place we can get hold of the router state in constructor cant accessit inside //implements OnInit { //
     
  {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras?.state?.error;
    //we r save bcoz we dont know if we r going to have any of this info bcoz when the user refreshes their page then we lose inside this navigation state get once when route is activated 
    //when we redirect the user to this server error page 
    // ? referred to as optional chaining operators //want to do check the extra 

   }

  ngOnInit(): void {
  }

}
