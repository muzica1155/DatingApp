import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
  
})
export class MemberListComponent implements OnInit {
//     members: Member[];// create a property to store a member // go & get the member list 
// //members:// only thing we can store inside here is an array of members 

//pagination// 
members: Member[];
pagination : Pagination;
userParams: UserParams;// we r going to need to get our current user bcoz when we initialize this particular class 
//UserParams;//what we need is our user details so that we can set the gender property inside 
user: User;//
 
genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];// we need a list a dropdown for the genders & what we'll do just initialize that will add a property called gender list
// & set this as an array & give it a value 

// pageNumber = 1; //& what we do inside here is we'll specify instead of page No & page size
// pageSize = 5;
//  totalItems = 10;
// members$: Observable<Member[]>;  //Observable member array 
// constructor(private memberService: MembersService, private accountService: AccountService)// inject our accountservice
constructor(private memberService: MembersService)//after pagination, filter & sorting

//we can set our user parameters inside this construct but we r going to get //userParams: UserParams;

 {// go & get our current user

  this.userParams = this.memberService.getUserParams();//& we r still updating these insider components So if user selects a FILTER then we r going to need to update the user parameters inside the service what we do inside our loadMembers
  // this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
  //   this.user = user;
  //   this.userParams = new UserParams(user); // creating new UserParams instance for our user Params // bcoz thisis a class we need ti instantiate it & will pass in the user as its parametere
  // //
  // })
  }

  ngOnInit(): void {
    // this.loadMembers();
    // this.members$ = this.memberService.getMembers();
    this.loadMembers();
  }
  loadMembers() { 
    this.memberService.setUserParams(this.userParams); //before we actually go & get the members we do 

    // then we also need to take care of our reset filters here & we need to reset this from the service as Well create a another helpre method in member sevice

    /// for Filtering // bcoz our load members method is now needing to pass different paramerters 
    // this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(response =>  //dont worry about piping this & taking 1 in this case this is a http response & http response is typically complete 
    this.memberService.getMembers(this.userParams).subscribe(response =>
      /// it not compulsory to use pipe to go & just take1 
    // our response now is our paginated results & if we take a look at thia then we 
    {
      this.members = response.result;
      this.pagination = response.pagination;// we got this info stored inside our components class properties
      
    })  
  }

    resetFilters() //create a method inside here to reset filters 
    {
      // this.userParams = new UserParams(this.user);// so that parameters get reset to what they r initialized at 
      this.userParams = this.memberService.resetUserParams();// then we can just go ahead & load the members again & what we also need to o in our pageChanged() method

      this.loadMembers();
    }

  pageChanged(event: any) {
    // this.pageNumber = event.page;
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);//after we have got the page number from the component we'll 
    this.loadMembers();//so that we go & get the next batch of members from our API service in place 
  }

  // loadMembers() 
  // { //getMembers().subscribe//its an observable that we r returning from this we need to subscribe and that
  //   this.memberService.getMembers().subscribe(members => {
  //     this.members = members;
  //   })
  // }

}
