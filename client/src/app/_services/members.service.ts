import { HttpClient } from '@angular/common/http';
//import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  //getMembers(): Member[] //not working bcoz we r not returning a members arrey fromthis what we r returning
  // Observable<Member[]>//
  getMembers() //Observable<Member[]>{// to much specified type too many place here
   {
    return this.http.get<Member[]>(this.baseUrl + 'users');///using typescript inference if not than add specific types wat we need is most amount of type safety the most amount of intellisense with the least 
  }//+ 'user')// members endpoint is users //httpOptions)// pass option we need to get members 
 ////get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server
  getMember(username: string) //when we see this is be explicit bcoz there is a danger that r get mmeber method would take a member as parameter and want to tell us if we do that mistake make sure only going to receive a string as its parameter 
 { 
   return this.http.get<Member>(this.baseUrl + 'users/' + username);
 }

}//get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server






