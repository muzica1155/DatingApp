import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {//want to do implement a control value accessor in register.component.ts//username: new FormControl('',//want to access this form, control inside our text input to whichever form control 
//that we r accessing on our templates via these form controls want to access these controls what we use ComtrolValueAccessor
//
@Input() label: string;//
@Input() type = 'text';// bcoz its text input so that we can specify the type of filed this is 
//if we want to override this then we'll simply pass in a type input property to our control as well now need in this component inject the control into the constructor of the component
  constructor(@Self() public ngControl: NgControl) //special decorator called Self what angular will do when its looking at dependency injection is oging to look inside the hierarchy of things that it can inject 
  //if there an injector that matches this to this already got inside its dependency injection container then its going to attem reuse that one dont want that to happen for this 
  //we want our text input component to be self-contained & we dont want anguar to try & fetch us another instance of what we r doing here 
  // we always want this to be self-contained this decorator ensures that angular will alwys inject what we r doing here locally into this component
  //(@Self()) //
  { 
    this.ngControl.valueAccessor = this;//& by adding this now we have got access to our control inside this component when we use it inside this componenent when we use it inside our register 
  }
  //all this methods have to to inside ControlvalueAccessor
  writeValue(obj: any): void {
    
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {

  }
    
  // setDisabledState?(isDisabled: boolean): void {
  //   throw new Error('Method not implemented.');
  // } not a required methods 

  // ngOnInit(): void {
  // }//we will remove the ngOn Init bcoz we r no longer implementing the on in it interface 

}
