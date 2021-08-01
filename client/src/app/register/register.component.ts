import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent: any; // input property//
  @Output() cancelRegister = new EventEmitter(); //EventEmitter commonly used to name for smthing // its a class we need to add parenthese there
  // used for button in the register form doesn't do anything doesn't affect the fact of whether we can dsiplay or not display the register toggle bcoz the register toggle is part of our component
  //& goal is this inside our register component html we have cancel button when we click want to do is inside our home compoenet the parent we wan tot change this to that we turn it of & we set this back to force //registerMOde =false; {in home.conponent}//
  model: any = {};

  constructor(private accountService: AccountService, private toastr: ToastrService) //bring in the account service here so that we can inject the account service into this component and that we need the account dervoce & call the register method will pass in this model & most subscribe there
  { }

  ngOnInit(): void {
  }

  register()
{
  this.accountService.register(this.model).subscribe(response => { //we r subscribing to a response that er get back from this 
    console.log(response);
    this.cancel();//we haven't got any rooting setup we just go for use conditionals to display and hide componenets
  }, error => {
    console.log(error);
    this.toastr.error(error.error);
  })
  // console.log(this.model);
}

cancel() {
  this.cancelRegister.emit(false);
  // console.log('cancelled');// we want we click on the cancel button we want to emit a value using this event EventEmitter
}
}
