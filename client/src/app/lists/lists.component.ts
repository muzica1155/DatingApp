import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
members: Partial<Member[]>;
  // create a property to store our members in now our members r going to be not quite full mumber we used it before when we were configuring the date picker
 //but we specify partial so eac 1 of the properties inside the member is now goignt to be optional & Member as well

 predicate = 'liked'; // we also add in here is the predicate & this is going to be a type of string but we set this toa default & we'' set this to liked
 pageNumber = 1;
 pagesize = 5; //
 pagination: Pagination;

 constructor(private memberService: MembersService) { }

  ngOnInit(): void {                             //we neeed to crate a properties
    // before testing we initiaize this 
    this.loadLikes();//some of these r displayed when we go to our page 
  }

   loadLikes()// we add a method called load likes
   {
     this.memberService.getLikes(this.predicate, this.pageNumber, this.pagesize).subscribe(response => {
      // what we do for get likes is pass the list of page nuber & list of pain sites 

     

       this.members = response.result;// we got error here bcoz lets see our getLikes returning unobservable of objects & we r going to need to update this so that we return the correct type 
       //we'll headck to our member service to update after changes error goes away 
       this.pagination = response.pagination;
       //bcoz we r now returning a paginated result the members r stored in the response.result & pagination is stored in the response.pagination
     })
     //what we need to use pagination we need our page changed event so we'll specify PageChanged event give type of any
   }
   pageChanged(event: any)
   {
     this.pageNumber = event.page;
     this.loadLikes();// fro here we go to memberlists component
   }

}
