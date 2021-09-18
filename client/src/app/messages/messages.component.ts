import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: Message[] = [];// add properties for messages // say message that we get from our interface not from angular & we'll also bring in the pagination or 
 //but we r using a length property in here just as we will do in here to see if we've actually got  any messages. in member-detail.component.ts
  pagination: Pagination;
container = 'Unread';// unread by default same as on the server & will add a page no = //starting container
pageNumber = 1;//
pageSize = 5;

loading = false;//we need to add a flag inside our message component to say that we r loading what we'll do we'll hide everything or behind the messages until 
//everything ready just add a loading flag

  constructor(private messageService: MessageService) 
  { }

  ngOnInit(): void {
      this.loadMessages(); // also initialize as well

  }

  //we want ot get the messages let create a method to get messages
  loadMessages() {
      this.loading = true; // then when we go & load our messages we what we r going to do is say Once we have finished loading them we r gonna say


    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
       this.messages = response.result;// this is going to paginatedResult response
       this.pagination = response.pagination;
       this.loading = false;// just do inside the method that we use to go & get the messages once we have them we r gonna turn off loading 
       //then go to the messagecomponent.html
    })
  }

  deleteMessage(id: number) //Add another method for deletemessage
  { 
    this.messageService.deleteMessage(id).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1); //used splice method here & we'll say lets stop messages & we'll find the index of the message & 
// paas the message ID m let expression & say = to the ID add comma specify how many messages we wamt to delete & in this case is just 1
// more thing need to do in message component templates 
    })// we r gonna take in the ID & they r not gonna subscribe & we dont get anything back from delete so just empty parentheses & will say
    //
    //
  }

  
  pageChanged(event: any)//just add page chenged event as well bcoz add pagination to this 1 as well
  {
   if(this.pageNumber !== event.page) {
     this.pageNumber = event.page;
   this.loadMessages;
  }

}
}
// pageChanged(event: any)//just add page chenged event as well bcoz add pagination to this 1 as well
//{
  //this.pageNumber = event.page; 
 // this.loadMessages;
 //}
//}







