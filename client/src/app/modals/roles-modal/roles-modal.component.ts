import { Component,EventEmitter, OnInit, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent implements OnInit {
  @Input() updateSelectedRoles = new EventEmitter();// we r fonna receive smthing from our component //EventEmitter// we bring this from angular core
// bcoz we wna tot admit the roles from this particular component but also need access to our user 
  user: User; //
  roles: any[]; //
  // title: string;//property
  // list: any[] = [];// list should be any array & just intitialize so we didn't get an error'
  // closeBtnName: string; // this properties when we create the instance of the model we r gonna pass these across with our model reference
  // //& this goes into our model & will display these properties that we can provide 

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  updateRoles()// add a method update the roles 
  {
    this.updateSelectedRoles.emit(this.roles);//& we'l get to the rolse model templates & make some  adjectments 
    this.bsModalRef.hide(); // fater this go to roles-model templates &
  }
//after setting the admin service comes to roles-model componet & now we need to see how we can pass all of htis from a component to our modal 
}
