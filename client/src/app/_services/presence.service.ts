import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);//we gonna create an observable 
  onlineUsers$ = this.onlineUsersSource.asObservable();
  
  //for this we use behavior subject & this is a variant of subject & the subject is a generic observable 
  //& this 1 requires an initialvalue & emits it's current value whenever it is subscribed  to So this q gonna work for what we r doing here 
  //we use the behavior subject & this is gonna be string Array of our username & initialize this to an empty array & then create a property
  //called online users & we;ll say dollar bcoz this is gonna this is gonna be observable & we'll say this to onlineUser source ....

  constructor(private toastr: ToastrService, private router: Router) { }

  createHubConnection(user: User) 
  {
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + '/presence', 
     {
       accessTokenFactory: () => user.token }) 
       
     
      .withAutomaticReconnect()
     
      .build()

      this.hubConnection 
      .start()//& this
      .catch(error => console.log(error)); 
      this.hubConnection.on('UserIsOnline', username => {
         this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
           this.onlineUsersSource.next([...usernames, username])//spread operator & pass in our username
         });//replace it with just an ability to update the online users that we r tracking inside here// once again we dont want
         //to mutate data here jus tn case that interferes with how angular tracks changes in our observable
        // this.toastr.info(username + ' has connected');//remove the annoyance from our app by removing toast
        
      })
      this.hubConnection.on('UserIsOffline', username => {
        this.onlineUsers$.pipe(take(1)).subscribe(usernames => { // username this is gonna be username that's gonna offline 
          this.onlineUsersSource.next([...usernames.filter(x => x !== username)]) //& tis would remove them from a list of online users so lets make sure we haven't actually broken 

        })
          // this.toastr.warning(username + ' has disconnected'); 
          
      })
      this.hubConnection.on('GetOnlineUsers',(usernames: string[]) => {
        this.onlineUsersSource.next(usernames);// then we can update ou behaviour subject with current list of online users 
      })
      //(username)// which is gonna be our string array 
      //after this go across to our components we';; do this in the backyard componenet & do this in member card componenet 
      //& we'll also add an indication in the member details component if a user is indeed online 

      this.hubConnection.on('NewMessageReceived', ({username, knownAs}) => {
        this.toastr.info(knownAs + ' has sent you a new message!')
        .onTap //inside here for these toast we can also root them to a new location/ so we onTap 
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/' + username + '?tab=3'));//+'?tab=3'))// just add the query string directly which is gonna take them directly to the messages of the user that has sent them the message or thats the theory
      })// bcoz this is what gonna happen if the users not connected to the same message group create a method
  //('NewMessageReceived',// what we r receiving is the object with the username

    }
       
       stopHubConnection() {
      
        this.hubConnection.stop().catch(error => console.log(error));
       
      } 
  }

