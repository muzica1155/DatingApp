import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;//this gives us access to this tempelete form inside our component
  //even through we have submitted our changes what we need to do is reset our form state in order to to reset the form inside our components
  //add two properties to fetxh the data specific data of particular user to edit 
  member: Member;
  user:User;//do is populate this user object with our current user from our account service the current user is anobservable and what we need to do is get the user out of that observable & use that for our user 
  //we need to access this object inside our component and we cant really work with an observable to do 
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm.dirty) { //if we want to do smthing before the browser is closed then we got an option to do so by this host listener 
      $event.returnValue = true;
    }
  } //allows us to do is access our browser events & the browser event that we want to access is the window before & load gives this and events so we'll say in quotes 
  //edit data & saved the data user accindently click the router & goes to google or any website it should show thow the prompt when we shift insie the our website from member to list to save the data or not 
  constructor(private accountService: AccountService,
     private memberService: MembersService, private toastr: ToastrService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);//now our user will have the current user from our account service 
  }

  ngOnInit(): void {//we initialize this component this component 
    this.loadMember();
  }
  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    })
  }
  updateMember() {
    // console.log(this.member);
    this.memberService.updateMember(this.member).subscribe(() => {
      this.toastr.success('Profile updated successfully');//if they updated their profile successfully,
      this.editForm.reset(this.member);//after we update our member// in order to keep & presence the values that we have enough 
    //this.editForm.reset(this.member);//this is going to be updated member after we have submitted our form 
    //but we dont have the API functionality yet 
    }) // we r going to get anyting back from this just add empty parentheses
    
  }

}
