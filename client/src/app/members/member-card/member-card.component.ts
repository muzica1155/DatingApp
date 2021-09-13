import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
  //encapsulation policy
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member;
  //what we need this member card component is an input properties bcoz we r going to be receiving
  //this data from its parents (memberlist component)

  //we add functionality in say that a user can like another user using members.service.ts
  constructor(private memberService: MembersService, private toastr: ToastrService) 
  { }

  ngOnInit(): void {
  }

  addLike(member: Member) //add a method we say add like & we'll take in a member of member 
  {
    this.memberService.addLike(member.username).subscribe(() => //we r not returning anything from the API that we r going to use inside our client here so we have just empty parenteses
     {
       this.toastr.success('You have liked ' + member.knownAs);// we dont need to do anything with the error bcoz our interceptor is taking care of that for us 
     }
    );
  }

}
