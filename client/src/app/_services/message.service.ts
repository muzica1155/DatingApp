import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Group } from '../_models/group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';


@Injectable({
  providedIn: 'root'
})
export class MessageService {
   baseUrl = environment.apiUrl;// need our base Url in here //baseUrl: string;// not coon here bcozwe r assigning smthing to this property & need our 
   //environment 
   hubUrl = environment.hubUrl;// add hub connection for our messages
   private hubConnection : HubConnection;
   private messageThreadSource = new BehaviorSubject<Message[]>([]);//what happens when we receive the messages when we connect to this hub FOr that we create an observable for this & we'll say private message thread
//<Message[]>// store message array//([])// intitialize empty array 
   messageThread$ = this.messageThreadSource.asObservable();//messageThread$// messageThread property signfy this is observable


  constructor(private http: HttpClient)// insdie here we wna to do is inject Http into contractor 
   { }

   createHubConnection(user: User, otherUsername: string)
   {
     this.hubConnection = new HubConnectionBuilder()
     .withUrl(this.hubUrl + '/message?user=' + otherUsername, {  //'/message')& here we want to pass up the other username as a query string with the key of users 
    
        accessTokenFactory: () => user.token
     })
     .withAutomaticReconnect()// so that client automatic reconnect so the client can try & reconnect themselves
     .build()
     this.hubConnection.start().catch(error => console.log(error));

     this.hubConnection.on('ReceiveMessageThread', messages => { // we need to target our receive message tthead// we r gonna get the message thread when we join a message group 
      //but what we want to make sure of is that if we join another message group then that user automatically marks the messages as read bcoz when we join it that means we r reading the message at the sametime
      // But now we've got a new method to listne for 
      this.messageThreadSource.next(messages);
       
     })//after this take care of componenet bczo we r gonna need to create this connection & receive the messages also need to deal with stopping the connection when the user moves away from the member details componenet 
       
     this.hubConnection.on('NewMessage', message => {// get the message from the hub // any new message that a user receives is automatically gonna be marked a snet ehich is fine 
           // slightly careful with the we do to add on any messages to this particular array//<Message[]>([])// so everything behaves correctly
           //& we gonna do is not u take this particular array we r gonna to get hold of the messages we r gonna add the new message on & 
           //then we r gonna update the behavior subject with a new message array that includes the message we've just received 
         this.messageThread$.pipe(take(1)).subscribe(messages => {
           this.messageThreadSource.next([...messages, message]);//the way that we can do this without mutating the array is to make use of the spread operator 
           //inside here//next()//
           //now this is gonna create a new array that's gonna to populate our behavior subject So we r not gonna to be mutating state inside here
         // we also need to do is take care of our send message

         })//subscribe to our message thread inside here which is an observable 
     })//add another this hub connection on method see 

     this.hubConnection.on('UpdatedGroup', (group: Group) => {
      // take look inside our message thread & see if there's any unread messages for the user that just join thr group & 
      //if they r then we we r gonna mark them as read inside here
      if (group.connections.some(x => x.username === otherUsername))//otherUsername// that we pass in to this particular method 
      {
         this.messageThread$.pipe(take(1)).subscribe(messages => {
           messages.forEach(message => {
             if (!message.dateRead) {// so if it's not been read then we r gonna to say message ....
                message.dateRead = new Date(Date.now())
             }
           })
           this.messageThreadSource.next([...messages]);//spread operator// pass in the updated messages now 
           //But this creates a new array so it shouldn't interface with angular change tracking 
         })//remember same technique & dont want to mutate this particular array of messages & subscribe & will get the messages
      }
    })
   }


   

   stopHubConnection()
   {
     if (this.hubConnection)
     {
      this.hubConnection.stop();//wrap this in safety condition so that we only try & stop it if it is actually in existence & this will prevent us 
      //from running inot any problem when doing either of those 2 operations 

     }
     
   }

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

   async sendMessage(username: string, content: string)///add an method to go & send a message to call it send message 
   {
    return this.hubConnection.invoke('SendMessage', {recipientUsername: username, content})//what we want to do instead is use our hub to do this 
   // then the way that we execute a method on our server is to use the invoke & this invokes a hub method using the specified name & arguments
   //& the promise returned by this method resolves when a server indicates it has finished invoking the method 
   //'SendMessage'// now this need to match exactly what we've called this on the server then we gonna to pass up the same details that form 
   //our create message Dto & bcoz now this os not return observable this is returning a promise 
   .catch(error => console.log(error));//bcoz we dont have access to our error hanading interceptor bcoz this is no longer an HTTP request
   //we r no longer returning observable from this method & we've some ellipses on sendmessage this tell us this may converted to an async function 
//  async sendMessage// this meansis that anything that's decorated with async we can use await operators inside here if we wished but the code as it is 
//this means as well is that we guarantee that we return a promise from this method Simply by adding this astnc keyword & we need to use
//the promise let's pretend bcoz in our member message componenet we r resetting the form as well once the message has gone 


    //  return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content});// we r makin in API call 
     // we know we r going to get a messageDto back from this we passs in <Message>
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





