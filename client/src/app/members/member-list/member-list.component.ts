import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
  
})
export class MemberListComponent implements OnInit {
    members: Member[];// create a property to store a member // go & get the member list 
//members:// only thing we can store inside here is an array of members 
  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() 
  { //getMembers().subscribe//its an observable that we r returning from this we need to subscribe and that
    this.memberService.getMembers().subscribe(members => {
      this.members = members;
    })
  }

}
