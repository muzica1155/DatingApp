
import { Message } from 'src/app/_models/message';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { MessageService } from 'src/app/_services/message.service';
import { MembersService } from 'src/app/_services/members.service';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm; // then come down to send message
  // @Input() username: string; // instead of using username here we r just gonna pass down the messages as an input properties
  @Input() username: string; //we gonna get this from our member detail component now go to member detail component template
  // messages: Message[];// change this
  @Input() messages: Message[];// changed the input properties of the messages & the we can pass them down //messages: Message[];// 
  //down to our member messages component

  //we also need to do is switch back to our member detial component 7 then instead of passing down the username
  //<app-member-messages [username]="member.username">// in member-details.component.html

  // we r going to go an input inside here 
  /// waht we need here really is just a username what's the username of the member that we've just clicked on? bcoz that what we r going to 
  //send up to go & gwt the thread itself 
  messageContent: string;// add the properties messagecontent is a type of string 
  

  // constructor(private messageService: MessageService)
  constructor(private messageService: MessageService)
   { }

  ngOnInit(): void
   {  
     
    
    // when we initialize this will say let' not load messages
    // this.loadMessages();
    // not using it later 

  }
  sendMessage()//crate a method inside here to actually send a message So 
  {
    this.messageService.sendMessage(this.username, this.messageContent).subscribe(message => {
      this.messages.push(message);// push the new messages that we've created inside here so that we can see it in the aray of the messages
      //& then what we need to do is go to our member message component template
      this.messageForm.reset(); //after we send a mesage we r gonna say// we just clear the content inside the form 

    })// we need access to the username which we also removed from  this as well 
    // we have our username now we also want is this message content which will add to our properties & will say 
    //after subsrcibe we need to get the message back fro this
  }
  
  
  
  
  
  //lets goes to template
  // loadMessages() // just have a method to load the messges from
  // {
  //   // bcoz we r notdoing pagination we just get this inside 
  //     // the body of the response we dont need to pluck itout of these
  //   this.messageService.GetMessageThread(this.username).subscribe(messages => 
  //     {
  //       this.messages = messages;
  //   })
    
  // }

}








