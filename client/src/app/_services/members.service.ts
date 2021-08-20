import { HttpClient } from '@angular/common/http';
//import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';// we r going to return smthing of an observable
import { map } from 'rxjs/operators';
// import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

// const httpOptions = {
//   headers: new HttpHeaders({ 
//     Authorization: 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token 
//   })//unspohisticated code we got our token being stored in to be options and then we need to send it with every request we make from our service not particularly clean

// }// our endpoint is protected by authentication which is users ///get token from localStorage

@Injectable({
  providedIn: 'root'
})
export class MembersService {//services are singleton they're instantiated when a component needs the services and that it operates as a singleton and it stays alive until the application is closed 
  //services make a good candidate for storing ur app state 
  //there are other state management solutions there's such things as ReDucks u could add to ur app here you could add mobacks toomuch for this app no need angular has services and redacts or molbeck would be overkill 
  // use our service to store our data service
  baseUrl = environment.apiUrl;
  members: Member[] = [];//if we get info from our members then that what we r going to return 

  constructor(private http: HttpClient) { }
  //getMembers(): Member[] //not working bcoz we r not returning a members arrey fromthis what we r returning
  // Observable<Member[]>//
  getMembers() //Observable<Member[]>{// to much specified type too many place here
   {
     if (this.members.length > 0) return of(this.members); //dont forget we need to return this as observable bcoz our client our component is going to be observing this data
    //we can use of operator from Rxjs
    //if we have the members then we can return the members from the service as an observable than set the members once we have gone & made the effort to get them from the API 
     return this.http.get<Member[]>(this.baseUrl + 'users').pipe
     (
       map(members => {
         this.members = members;//the map operator returns the values as an observable when we return from the map we r returning these as an observable doing same in both methods 
        // we r returning unobservable of the members from here & the same from here as well 
       return members;
      }));///using typescript inference if not than add specific types wat we need is most amount of type safety the most amount of intellisense with the least 
  }//+ 'user')// members endpoint is users //httpOptions)// pass option we need to get members 
 ////get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server
  getMember(username: string) //when we see this is be explicit bcoz there is a danger that r get mmeber method would take a member as parameter and want to tell us if we do that mistake make sure only going to receive a string as its parameter 
 { 
   const member = this.members.find(x => x.username === username); //get mrmber from the members we have inside our service. we still need to accommodate the fact that the user might refresh their page and we dont have anything inside our service 
   //find(x => ) //find is the memer with the same username that we r passing in as a parameter
   if (member !== undefined) return of(member); //
   //go & take a look at what the find method returns is it does not find what we r looking for find method here & find casue a predicate once for each elements of the arry in ascending order until it finds one where the predicate returns true if such an element is found it immediately return the elements value otherwise find retuns undefined
   // what we r looking fro comparison here we r not looking to compare against now we wnat to look for undefined 
   //if we dont have the member then we r going to go & make our API call
   return this.http.get<Member>(this.baseUrl + 'users/' + username);
 }
  updateMember(member: Member)// add a method inside here 
  { return this.http.put(this.baseUrl + 'users', member).pipe(
    map(() => {
      const index = this.members.indexOf(member);//find member that matches that member in our index
      this.members[index] = member;//this go across to our member lists component 
    })//we dont have anything coming back from our http put method our server doesn't give us a response 
    // get the member from the service 
    ) //pass member as object
   //also update our update member bcoz if we r getting our members now from our service then if ew update a number & dont do anyhting with that they go back they r going to go & see the old data that's stored inside here 
   //so we also need to update this when we update our member as well 
  }

}//get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server






