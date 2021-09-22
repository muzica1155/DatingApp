import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
   users: Partial<User[]>;// add a property

   bsModalRef: BsModalRef;// we need to add a reference to a middle

  constructor(private adminService: AdminService, private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles() // i did a classic school boy error creating a method & then forgotting to add it to the NgOnInit
  {
    this.adminService.GetUsersWithRoles().subscribe(users => {
      this.users = users;
    })// admin service that we need to inject 
  }
  openRolesModel(user: User)// create a method 
  {
    // const initialState = {// ithat list inside here we r passing the initial states & we r passing that list & the 
    //   list: ['Open a modal with component', 'Pass your data','Do something else',  '...'  ], title: 'Modal with component'//we r passing that title 
    // };// removed the initialState after edit adminservice 

    //set config as an object 
    const config = {
      class: 'modal-dialog-centered',//give some configuration properties
      //'modal-dialog-centered'//this will & this will ensure that our model appears in the center of the browser screen 
       initialState:   //then what we'll do is we'll give some initial state 
       {
         user,// we r gonna pass the user //pass in user into the method parameters 
         roles: this.getRolesArray(user) // we need to create a method bcoz thisis going to need some logic inside it & we'll remove this initial state & swap this config
         //so this role's we need to know which roles the user is in so that we can populate the checkboxes that r already checked 
         //& for the roles that the user is not in we need to make them available to be shown inside the modal 
         //In an unchecked state so what we'll do is we'll create a method a private method in here called Get roles Array
     
       }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    // this.bsModalRef = this.modalService.show(RolesModalComponent, {initialState});//here u can pass in some configuration if we have it 
    //{initialState});//passing intial state in this object gonna have acces to the list properties they still need to be defined in the roles model themselves
    // we still need them in our role-model.componenet we have acces to tem inside that 

    // this.bsModalRef.content.closeBtnName = 'Close';// this is just an example of passing data nother way via both mobile content
    // we also can get access to the content inside there as well as giving it osme initial state 

    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {  // = to object 
        roles: [...values.filter(el => el.checked === true).map(el => el.name)]// here we use spread operator to spread the contents of the values 
        //gonna filter out anything that hasn't been checked
        // we r gonna say rolls & then we r gonna to set this to all of the values that have beeen checked that we get back from our modal 
        // so we gonna have this inside our values Everything that been checked is gonna e inside there
      };
      if (rolesToUpdate)//then we check to make sure that we have smthing to update 
      {
        this.adminService.updateUserRoles(user.username, rolesToUpdate.roles).subscribe(() => {
          user.roles = [...rolesToUpdate.roles] //spread operator again
        })
    //(() =>)//inside the callback function 
      }
    })
    //go back to role-model.component we need to deal with this //updateSelectedRoles = new EventEmitter();//
    //bcoz we r going to get the roles that have been checked from this method back into our component// (this.roles);//
    //on this method// updateRoles()//back inot our componenet & we need to deal with what happens when that happens 
    

  }
  //In this particular model is we need to get all of that info Our roles model needs in order to work with what;s going on here 

  private getRolesArray(user)// create a method for roles //taken our user as a parameter here
  {
    const roles = [];//start of with empty array
    const userRoles = user.roles;//then we get hold of our user roles
    const availableRoles: any[] =//& then we set the available roles available to be selected // we will give this type of any array
    [
      {name:'Admin', value:'Admin'},// just pass in objects that r gonna make up the values that can be selected in our check box
      {name:'Moderator', value:'Moderator'},
      {name:'Member', value:'Member'},
    ];
    //after this we want ot loopover this availableRoles & find out if our use is a member of any of those availableRoles
    //if they r then we r gonna check box basicaly 
    availableRoles.forEach(role => {
       let isMatch = false;// we wann check to see if the user role matches this roles 
       for(const userRole of  userRoles)//then we loop over the roles of the user roles of the user role
       {
         //kind of a loop within a loop is what we r doing here & what we r gonna do is check to see if the roll 
         if(role.name === userRole) // === userRole// which is just the string that
         {
            isMatch = true;//then if it is a match we r gonna set this match = to true //& saying this propertied is true 
            role.checked = true; // users role it's never gonns be checked or ot not gonna be checked
            roles.push(role);//(role);// into our roleArray// then we r pushing th roles into this roleArray & we r still inside foreach loop at this atage unerneath this for loop
            break;//coz we r in a loop within a loop we gonna do break out of this 
         }
       }//below forloop 
       if (!isMatch)//check to see if the role is not a match
       {
         role.checked = false;
         roles.push(role);//
       }

    })
     return roles; 
  }//after this go back to top at initialState


}