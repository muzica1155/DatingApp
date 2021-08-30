import { Input, Self } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.css']
})
export class DateInputComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() maxDate: Date; //bcoz what we want to do example we want to say that u have to be over 18 to be able to use our website 
  //we'll add a max date to show the earliest dates that the date packable show & what we need to do is also do 
 bsConfig: Partial<BsDatepickerConfig>;//when we use partial we r saying here is that every single property inside this type is going to be optional 
 //we dont have to provide all of the different configurations options we can only apply or we could only provide a couple of them & that file
 //if we didn't use partial then we would have to provide every single possible configuration option they've put inside that so we use partial tto achieve that
 //
  constructor(@Self() public ngControl: NgControl) {
    //what we didi with the input we need to make 
  //constructor(@Self)//  make this self so that the dependencies 
  
    this.ngControl.valueAccessor = this;
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD MMMM YYYY'
    }
   }

  //thisis differnet complicated bcoz we r going to be using the data input component from ngx bootstrap 

  writeValue(obj: any): void {
    
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {
    
  }
  

  

}
