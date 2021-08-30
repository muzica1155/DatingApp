import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

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
  // model: any = {};
//Setup our Reactive form add a property here & call it register form
  registerForm: FormGroup;
  maxDate: Date;// before initialize the form create a another property her inside ngOnInit():
 validationErrors: string[] = []; //array string bcoz we know we r getting that back from our interceptors if there r such a thing & then what we can do if there is an error that we get thrown back from 
 // need to do is initialize these validation errors as well bcoz inside our register component we check checking for the length of the array 
 // if we dont initialize the array then we r going to get an error bcoz that will then be undefined & undefinrd doesn't have a length property 
  constructor(private accountService: AccountService,
     private toastr: ToastrService, private fb: FormBuilder, private router: Router) //bring in the account service here so that we can inject the account service into this component and that we need the account dervoce & call the register method will pass in this model & most subscribe there
  // FormBuilder)// we will use this to create our form 
  //bring router in constructor when user save & redirect to members page
     { }

  ngOnInit(): void {
    this.initializeForm();//we can initialize our form for that create a method
  //when we do initialize our component we have our form initialized 
  this.maxDate = new Date();
  this.maxDate.setFullYear(this.maxDate.getFullYear() -18);//this means that our date picker doesn't allow any selection of dates les than 18 yrs old then what we need to do inside our register component is is we also need '
  }
  //reactive form methods 
  initializeForm() {
    this.registerForm = this.fb.group({
    // this.registerForm = new FormGroup({
      // FormControl(''// instead of specifying new form controls inside here we can specify that this r simply arrays
      
      // username: new FormControl('', Validators.required),//specify username// fro validation we could give it a starting value we could give it an intial value
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required,
         Validators.minLength(4), Validators.maxLength(8)]],//specify password//empty string need to get access to the 2nd parameter here & if we wanted to add to validators then we we would out them inside an array
      //
      confirmPassword: ['', [Validators.required, ///typically create a react form using square
        this.matchValues('password')]],//specify confirm password// need to add a custom validator so that we can compare the password field with the confirmed 
    //sqaure bracket [Validators.required]// bcoz now we r going to be passing in an additional validator & 
    })//if u change the password after u have validated the confirm password u will find ;- it doesn't invalidate our form we need to add sm additional inside our initalize form method 
    //bcoz the confirm password validator is only applied to the confirmed password // what we need to check thepassword field again update it is valid the T against the confirmed password 
   this.registerForm.controls.password.valueChanges.subscribe(() => {
     this.registerForm.controls.confirmPassword.updateValueAndValidity();
     //what we r going to be testing for is when our password changes then we r going to update the validity of that field against this field as well if we add this in then we can validate against both of these fields 
     // & if either on of them changes & does not match theother one then we validate our form once 
   })
  }

  //for custom validator we need to create a new method 
  matchValues(matchTo: string): ValidatorFn {//all of our from control derive from an abstract control//that what we want to return 
    return (control: AbstractControl) => { //[matchTo] //to access the specific control that we r comparing against 
     return control?.value === control?.parent?.controls[matchTo].value
      ? null : {isMatching: true}// ? optional chaining the controls parents & this gives is access to all of the controls in the form & pass in the match 
  }// what we r doing here is we getting access to the control that we can attach to this validator to now we cannot attach this to our confirm password control
  //we r going to match to a string bcoz our field names are strings 
  //what we can do from this will actually specify a type of what we want to return 
  //& what we r returning here is a validator function//ValidatorFn//that return type from this particular method
  //null : {isMatching: }// if it does match we return a object & we isMatching 
  //this can be anything just an arbitary name say that yes this validator is passing & then we r going to say true 
  //control?.value //control here is to confirm password control & we r going to go up to compare this to whatever we pass into the match too & we r going to pass in the password 
  //that we want to compare this value to & if these password match then we return now & THAT MEANS
  //validation hass passed if the password dont atch then we attach a validator error called is matching to the control & then this will fail our from validation.

}
  register()
{
  // console.log(this.registerForm.value);// going to contain the value for all of these form controls 
  // this.accountService.register(this.model).subscribe(response => { //we r subscribing to a response that er get back from this 
  this.accountService.register(this.registerForm.value).subscribe(response => { 
    // console.log(response);
    // this.cancel();//we haven't got any rooting setup we just go for use conditionals to display and hide componenets
    this.router.navigateByUrl('/members');//
  }, error => { //what to do about errors bcoz register is a form & whilst we do have client side validations we also want to take care of validation from the sever side as well
    //just in case we had a mismatch between what we require in our form & what we require in our register form 
    // console.log(error);//if there is an error that we get thrown back from our Interceptor bcoz we r going to throw the arrow back in that case then what we do is we'll
     
    this.validationErrors = error; // that we get back & then inside r registered components templete 

    // this.toastr.error(error.error);// no need of toastr bcoz if we get a bad request that going to come back from our interceptor
    //but if we get a validation error 7 that gets passed back into this then we need to take care of that display that on our form 
  })//we want when a user registers is we will redirect them to the members page 
  // console.log(this.model);
}

cancel() {
  this.cancelRegister.emit(false);
  // console.log('cancelled');// we want we click on the cancel button we want to emit a value using this event EventEmitter
}
}
