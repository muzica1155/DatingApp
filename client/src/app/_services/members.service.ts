import { HttpClient, HttpParams } from '@angular/common/http';
//import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, pipe } from 'rxjs';// we r going to return smthing of an observable
import { map, take } from 'rxjs/operators';
// import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

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
  
  memberCache = new Map(); // a map is like a dictionary really 

  user: User;
  userParams: UserParams;

//pagination setting

  // paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();//set up a property for our patinated results // take our class parameters of our paginatedResult
 //paginatedResult:// this is what w egonna store our results in 
  constructor(private http: HttpClient, private accountService: AccountService)
  //private accountService: AccountService)//we have to be careful when injecting inside the service because of circular reference if we try to inject member service into our account service then we have problems as long as we dont do that 
   {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => { //we have to create a user property inside our service so that user of time user 
      this.user = user;
      this.userParams = new UserParams(user); // creating new UserParams instance for our user Params // bcoz thisis a class we need ti instantiate it & will pass in the user as its parametere
    //caching after pagination, filteration & sorting 
    //if we apply filter to a member then click on a member then go back to our matches we have lost our filter too
    //this.userParams = new UserParams(user); // what if we done this inside our service ? WOuld it be remembered that ?Of course our service r singletons & we could create these user problems inside our service ?
    //they are not going to disappear they r not going to destroyed Any time we move from this componenent to 1 member details componenent or anywhere else in our app they're gonna to be remembered 
    // we r going to inject our acount service into our member service & hten do effectively the same as hwa we r doing here so taht we create the user parems inside the service 
  
    //
    })
    }

     getUserParams() {
       return this.userParams; //also add another helper method called set userParams
     }// add helper methods so we can get & set these userParams

     setUserParams(params: UserParams) // we can give us some help & say that this is going to be a type of our userParams
     {
       this.userParams = params;
     }

     resetUserParams() 
     {
       this.userParams = new UserParams(this.user);//(this.user);// to reset the parameters & then we just return from this method so
       return this.userParams; // that our componenent has access to them the user program // 
     }

  //getMembers(): Member[] //not working bcoz we r not returning a members arrey fromthis what we r returning
  // Observable<Member[]>//

  //caching after paging //

  //retore caching when we click on user we retrive the user from cache & when we go back to our result then if were already loaded them then we r going to want to get that from memory now its complicated 
  // when we use filter, pagination & sorting the page user it should remember the query & what was stored specific query as no longer as simple as just remembering an array of members & make sure that if a user's asked for page 2 of male even they asked for age we want to resukt of what is stored inside here insdie our service 
  getMembers(userParams: UserParams)
  

// what info do we have or use //

//each one of these differnt types of query whether we r geting a list of females or male all info we get to specify whay type of query this is 

//what can to be sending up to our API is stored inside this userParams//u& then for each key each query we can store the response 

  // getMembers(page?: number, itemsPerPage?: number) //Observable<Member[]>{// to much specified type too many place here
  //pagination// getMembers(page?) specify page which can be null we'll at the optional parameter there going to be type of number & then we'll have our 
  
  //(page?: number, itemsPerPage?: number)// there is lot more thing to pass in now make an object when u get more than three or four propertirs that u r passing in as parameters it gets really messy
  //create a class to store our userPrem's on the client as well 
  {
   var response = this.memberCache.get(Object.values(userParams).join('-')); // when we using a map we can set & we can get from this & we store everything with a key 
  //ew can use the same key that we used to set as we used to get //join('-'))//yep thi si the key & then if we have a response in our cache, then we can return it , ust as we did earlier & we'll say if we do have a response for that key 
   // we r going to check to see if we have in our cache the results of that particular query 
   if (response) {
     return of(response);
   }
 // then what we do when we get our result back we dod inside our //return this.getPaginatedResult<Member[]>//
//result & this is returning an observable of patinated result of member Array we know wha twe r returning & what we do inside here bcoz its an observable we need to 

  //   console.log(Object.values(userParams).join('-'));//bcoz userParams is an object// when we want to store smthing with the key & value a good thing to use is a map 
  // //use join method & will join each value by - 

  

    //  let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);// after pagination
     let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
     
     params = params.append('minAge', userParams.minAge.toString());// // pass in this ones that we wnat to adjust for this specific method // what our params r going to be for this is we r going to have a middle age & this is going to 
     
     params = params.append('maxAge', userParams.maxAge.toString());

     params = params.append('gender', userParams.gender);

     params = params.append('orderBy', userParams.orderBy);

     //add the additional parameters so that we send up the order by as another param in our query string 


    //     let params = new HttpParams(); // ability toserialize our parameters & thi is going to take care of adding this on to our query string
    //    if (page !== null && itemsPerPage !== null) //double check to see if we got a page & make sure its not equal to null & also double check that itmes perpage is also not equal to null
    //        {//otherwise we r just goingt o stick with the default & let the server return what he wnat to 
    //         params = params.append('pageNumber', page.toString()); //('pageNumber')// this is our query string 
    // //page.toString());//query string is a string we neeed to make th epage .toString()
    //         params = params.append('pageSize', itemsPerPage.toString());
    //        }

    //  if (this.members.length > 0) return of(this.members);
      //dont forget we need to return this as observable bcoz our client our component is going to be observing this data
    //we can use of operator from Rxjs
    //if we have the members then we can return the members from the service as an observable than set the members once we have gone & made the effort to get them from the API 
    //  return this.http.get<Member[]>(this.baseUrl + 'users', {observe: 'response', params}).pipe
    //  //pagination// 
    //  //this return method is we need to pass up our parameters here //users').pipe//
    //  //this kind changes bcoz when we r just using HttpGet normally thi si going to give us the response body when we r observing the response & we use this to pass up the parameters that we have created here then we get the full response back 
    //  //we dont it doesn't go & get the body of the response for us we all need to do that ourselves 
    //  (
    //    map(response => {
    //      this.paginatedResult.result = response.body;
    //      // our members array is going to be contained inside here 

    //       if (response.headers.get('Pagination') !== null)//check our pagination headers 
    //       {
    //         this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

    //       }
    //       return this.paginatedResult;// we r going to need to do things differntly inside member list component for now we r getting this memebers 

    //    })
    //   //  map(members => {
    //   //    this.members = members;//the map operator returns the values as an observable when we return from the map we r returning these as an observable doing same in both methods 
    //   //   // we r returning unobservable of the members from here & the same from here as well 
    //   //  return members;
    //   // })
    //   )///using typescript inference if not than add specific types wat we need is most amount of type safety the most amount of intellisense with the least 
   
    // return this.newMethod(params)

    // return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params)// we have to give this type parameters bcoz we specified a type here //getPaginatedResult<T>(url,// 
    //we need to tell it what type of pagiantedResult we r getting from this 

    return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http)

       // caching after pagination
       //we use a dot & then pipe 
       // so the idea is that we go to our API we go & get our Members if we dont have them in our cache 
       //But if we do have them in our cache in the query is identical then we just retrive this from our cache 
        .pipe(map(response => {
          this.memberCache.set(Object.values(userParams).join('-'), response); // when we use sets we give it a key which we have done here & then we specify thew value & the value going to be response
          return response;
        }))// we need to use the pipe & that inside here we can use the map method 
       
       // pipe(map)// this is another rxjs map & things get slightly confusing bcoz we r using a map object here
//memberCache = new Map();//Map(); this is a dictionary it's those values & key value format So our key is going tobe what we get here that
//we get here that we were looking at in the console & value is going to to be the response we get back from our server so what we use the map function & we use this to transform the data that we get back 

  }//+ 'user')// members endpoint is users //httpOptions)// pass option we need to get members 
  // private newMethod(params: HttpParams) 
//   private getPaginatedResult<T>(url, params) 
  
//   {

//     const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();//set up a property for our patinated results // take our class parameters of our paginatedResult

//     //const paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

//     // return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
//       return this.http.get<T>(url, { observe: 'response', params }).pipe( //we will make this generic swap this this.http.get<Member[]>(this.baseUrl + 
//       //return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
//         map(response => {
//         // this.paginatedResult.result = response.body;
//         paginatedResult.result = response.body;

//         if (response.headers.get('Pagination') !== null) {
//           // this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
//           paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

//         }
//         // return this.paginatedResult;
//         return paginatedResult;

//       })
//     );
//   }

//  ////get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server
 
//  private getPaginationHeaders(pageNumber: number, pageSize: number)// for easy to access 
//  {// what we going to reutnr from this is the Params
//   let params = new HttpParams(); // ability toserialize our parameters & thi is going to take care of adding this on to our query string
  
//   // //bcoz we r initialzing the page number and a page size we dont need to check to see if we 
//   // if (page !== null && itemsPerPage !== null) 
      

//   // {//otherwise we r just goingt o stick with the default & let the server return what he wnat to 
//       //  params = params.append('pageNumber', page.toString()); //('pageNumber')// this is our query string 
//       params = params.append('pageNumber', pageNumber.toString());
// //page.toString());//query string is a string we neeed to make th epage .toString()
//       //  params = params.append('pageSize', itemsPerPage.toString());
//       params = params.append('pageSize', pageSize.toString());

//       return params;
//       }

 
 
 getMember(username: string) //when we see this is be explicit bcoz there is a danger that r get mmeber method would take a member as parameter and want to tell us if we do that mistake make sure only going to receive a string as its parameter 
 { 
  //  const member = this.members.find(x => x.username === username); //get mrmber from the members we have inside our service. we still need to accommodate the fact that the user might refresh their page and we dont have anything inside our service 
  //  //find(x => ) //find is the memer with the same username that we r passing in as a parameter
  //  if (member !== undefined) return of(member); //this code is commented bcoz after pagination & caching we r not using this code bcoz there r no members in the members array 

//  console.log(this.memberCache);// how to find individual member inside this ? //= new Map();// we know it's definitely in there but what we also know is that our member cache affects lets just log out the member cache let take a look what we r working 

const member = [...this.memberCache.values()]// we do is get all the values of the MemberCache wouldn't need the keys now we just need the values & we know that our users r stored in the values of that 
//we used spread operator //[...this.memberCache.values()];// then console.log() & see what we have 

// console.log(member);//go to browser see the console u get 2 PaginatedResult then age from 18: 60 apply filter click on 1 user then see console u get 3 result 

//each 1 of this contain array where the results of our users so what we really need is a single array that we can use to find a particular user just like we had before how do we do that ?

//complicated method used called reduced method whatwe use this for is to reduce our array into smthing else we dont want an array of arrays with the opportunity objects 7 other things inside it 
//we just want the results of each array in a single array that we can then search & find the 1st member that marches a condition & we can use this reduced function to achieve that condition

// we call the specified callback function so we give this a function a callback function for al of the elements in an array & the return value of the callback function is the accumulated result & is provided as an argument in the next call to the callback function and
//& it accepts up to four arguments & reduced method calls the callback function 1 time for eachn element in the array 
//.reduce(() =>)//now we need to give this a callback function this is how callback functions looks like this // this is how we start any function & then we say what we wna tot do to each array element we have 
//.reduce((arr) => )//we r going to do is we have got access to precious value here & then we have got access to a current value we say that our previous value is just an array as// arr //
//.reduce((arr, elem) => )//then current value we apply this callback function to each element in the array elem for elemets 

//also do to give our reduced function & initial value we can give this an intial value// .reduce(( //& if an initial value is specified it is used as the initial value to start the accumulation 
//we r going to use a method on the array the previous value & also we want to do is concatenate the array together use array concat //arr.concat(elem.result)//then we can passin the elements dot results
//(elem.result), [])//bcoz or members were contained inside the results of the array & then we give our reduced function & initial value whichn is just an empty array 
.reduce((arr, elem) => arr.concat(elem.result), [])
// so as we call this function on each elements in the array we r going to get the result which contains 5 members, 2 members, 4 members, however many r in our cache
//& then we r goingt ot concatenate that onto the array that we have which starts with nothing [], see wht we get by console.log for

// console.log(member);//we got to know it possible our users could be duplicated in this array bcoz if i filter between 2 user ages from 10 to 12 list of array we get having 2 lisa bcoz there r 30 different queries we got list of that stored in our age filter that we used & we have also got a piece of it stored in our array of five Users
// that didn't use the filter bcoz we r going to use a method that going to find the 1st elements in array that matches the name that we r looking for 
// now we have got our users flattened into a array now we use find method 
.find((member: Member) => member.username === username);// what we r looking for is a member going to be of type member but we have to put this in parentheses member of type member bcoz that's what in our array & then 
//now we have check to see if we member
if (member) // if we do we r going to return of an passing a member 
{
  return of (member);//& our find method is going to look for the 1st instance of this & reurn this to us SO in theory what we should be able to look 
}



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

  setMainPhoto(photoId: number) //add a method to set the user's MainPhoto
  {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});//bcoz its a put request we do need to send smthing in the body but what we can send up here is na empty object
  //& that will suffice for our service for the time being 
  }
  deletePhoto(photoId: number)// add a new method in here to delete the photo & well go to photoID
  { 
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);//Concatenated ur photo straight on to the this method here//'users/delete-photo/' + photoId);// 
  }

  //add 3 new methods for likes member 
  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {})//username & this is what we used to like a user now bcoz this is a post we do need to add an empty object in this case
  // htis matches exactly what we r doing in postman 
  }

  //add a method to get likes & were going to take the predicate in as
  // getLikes(predicate: string)
  
  getLikes(predicate: string, pageNumber, pageSize)

  {
    // let params = this.getPaginationHeaders(pageNumber, pageSize);// what we can pass in  page number & page size now do we create another class like we did for our
    //params or do we do smthing different bcoz we only need 3 tings here? we need a predicate, page number, pageSize
    //skip creating the clss in this case we can go but we add these properties to the member or to the list component directly &
    //will add inside here predicate will take in the pageNumber & pageSize
    //getPaginationHeaders()// specify the page number& the page size inside here & 

    let params = getPaginationHeaders(pageNumber, pageSize);

    params = params.append('predicate', predicate);
    // also append('')// we need to specify the predicate & we r going to set this equal to the predicate that we r getting in our parameters here

  //  return this.http.get(this.baseUrl + 'likes?=' + predicate)//likes?//we wont use http to be Params at this stage add on the predicate to the url & like ?= that going to be either liked or liked by which is what we'll use for the prediccate 
  
  // return this.http.get<Partial<Member[]>>(this.baseUrl + 'likes?predicate=' + predicate); // we update this for liked list// we e going to gice this a hint about what it's returning
  
  // in this case it;s going to be returning Partial & member array
    //we r just going to get our members & we r passing the predicate directly onto the query string// now we need to pass in 
    //pagination headers as well take a look what we r doing for our get members then we have a method to go & get our 

    // this.getPaginationHeaders & also got method to return paginated resulkt as well//getPaginatedResult<Member[]>//

    // return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params);//that we need to doin the service //from here we r going to list componenet  
    //  // we also need to do with our paginated results was give this a type & we'll say partial in members.service.ts 
    // this should take care of the error tat we r seeing inside our lists componennt

    return getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params, this.http);

  }
////////////////////////////////////////////////////////////////
////////this code is commented for the message module///////////////
///////// bcoz we r going to need pagination for message.service.ts for this & we dont have the access to our methods inside here //
///////bcoz we created them as private method inside out member service /////////////////////////////////////////
/// what we could do take this 2 methods & put them into their own file but we need to think about 1 thing here that the paginated result//
//bcoz we need to use the Http //this.http.get<Member[]>//
// We can pass in http as a parameters inside this methods params //private getPaginatedResult<T>(url, params) ////
//then our service should injecting that then we could use that as a parameter in here ! way to go about it //copy & paste our member service
// pasted in paginationHelper.ts


//   private getPaginatedResult<T>(url, params) 
//   {
//     const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();//set up a property for our patinated results // take our class parameters of our paginatedResult

//     //const paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

//     // return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
//       return this.http.get<T>(url, { observe: 'response', params }).pipe( //we will make this generic swap this this.http.get<Member[]>(this.baseUrl + 
//       //return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
//         map(response => {
//         // this.paginatedResult.result = response.body;
//         paginatedResult.result = response.body;

//         if (response.headers.get('Pagination') !== null) {
//           // this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
//           paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

//         }
//         // return this.paginatedResult;
//         return paginatedResult;

//       })
//     );
//   }

//  ////get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server
 
//  private getPaginationHeaders(pageNumber: number, pageSize: number)// for easy to access 
//  {// what we going to reutnr from this is the Params
//   let params = new HttpParams(); // ability toserialize our parameters & thi is going to take care of adding this on to our query string
  
//   // //bcoz we r initialzing the page number and a page size we dont need to check to see if we 
//   // if (page !== null && itemsPerPage !== null) 
      

//   // {//otherwise we r just goingt o stick with the default & let the server return what he wnat to 
//       //  params = params.append('pageNumber', page.toString()); //('pageNumber')// this is our query string 
//       params = params.append('pageNumber', pageNumber.toString());
// //page.toString());//query string is a string we neeed to make th epage .toString()
//       //  params = params.append('pageSize', itemsPerPage.toString());
//       params = params.append('pageSize', pageSize.toString());
//       return params;
//       }


}//get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server






