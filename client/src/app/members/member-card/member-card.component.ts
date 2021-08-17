import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

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

  constructor() { }

  ngOnInit(): void {
  }

}
