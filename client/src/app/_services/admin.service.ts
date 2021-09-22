import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
   GetUsersWithRoles()// add single method 
   {
     return this.http.get<Partial<User[]>>(this.baseUrl + 'admin/users-with-roles');//exect what we were doing in postman 
     //when this is gonna return actually is a partial of our users 
     //get<Partial<User[]>>// bcoz we r only getting some of the properties of a user back from this 
   }

   updateUserRoles(username: string, roles: string[])//add a method into our adminservice so we can actually update the rules 
   {
     return this.http.post(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {})//post()// this time we wont give this a type 
     //'?')//? bcoz we r gonna to send these up as aquery string exactly the same as what we did in postman then we specify the roles
     //roles)// bcoz this is a post request  we r gonna need to add an empty object 
     //after roles we need to send to object bcoz this a post request 
   }
}
