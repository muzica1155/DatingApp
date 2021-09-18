import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';


@Injectable({
  providedIn: 'root'
})
export class MessageService {
   baseUrl = environment.apiUrl;// need our base Url in here //baseUrl: string;// not coon here bcozwe r assigning smthing to this property & need our 
   //environment 

  constructor(private http: HttpClient)// insdie here we wna to do is inject Http into contractor 
   { }

   getMessages(pageNumber, pageSize, container)// add a method to og & get our messages that's going to be our starting point //make sure that we can get messages we'll designt
   //our inbox & outbox etc... // when we get our messages in this is paginated SO we r going to need our page number
   {
     // we need pagination for this & we dont have access tto our methods inside here bcoz we created them as private methods inside our member.service 
     let params = getPaginationHeaders(pageNumber, pageSize);
     params = params.append('Container', container);
     return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
     //getPaginatedResult<Message[]>//i have called the interface message & angular useless in 1 of their modules top message mak sure its accute 
     //
   }

   GetMessageThread(username: string)// we r not add pagination to this // we r going t
   {
     return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);// & it;s not that we cant add pagination just try to avoid doing to much repetitive
     //repetitive tasl just introduce new things jusr concenreate on the thread & functionality inside there 
   }

   sendMessage(username: string, content: string)///add an method to go & send a message to call it send message 
   {
     return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content})// we know we r going to get a messageDto back from this we passs in <Message>
   //+ 'messages', {})//pass up the content not gonna create a type for this just gonna use & Object bcoz we r going to sned this inside an object
   //& they r not gonna to specify the name of the parameter or the key in this case & this is going to match what we r looking for in 
   //our create messageDto & has to match exactly gonna say recipientUsername & will set this to username & then er r gonna pass up the content
   // when we create an object to in this way Then if the name of the parameter or the name of the property is exactly the same as what we r
   //passing in we dont need to specify Content is content for instance we can just leave it as content 
   //But when it's differnt like it is in this case we 've used recipent username Then we r gonna to set that to the username & we do need to set that to 
   //add the value of the key in this case them what we can do is we can head over to our member messages, components 
    }

    deleteMessage(id: number)// another method inside here that gonna take care of the deleting of the message// gonna take id of a number for the message ID 
    {
     return this.http.delete(this.baseUrl + 'messages/' + id);// now got ot messages componenet 
    }
   
}





