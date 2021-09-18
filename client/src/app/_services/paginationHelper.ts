// now these r not inside a class anymore so thses r going ot need to be functions 

import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../_models/pagination";



//now thi
////////////////////////////////////////////////////////////////////////////////////////////////
// private getPaginatedResult<T>(url, params) 
// changed for class to function
export function getPaginatedResult<T>(url, params, http: HttpClient)// pass http in parameters 
{
  const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

  // return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
    // return this.http.get<T>(url, { observe: 'response', params }).pipe( //we will make this generic swap this this.http.get<Member[]>(this.baseUrl + 
    //return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(

        return http.get<T>(url, { observe: 'response', params }).pipe( 
      map(response => {
      // this.paginatedResult.result = response.body;
      paginatedResult.result = response.body;

      if (response.headers.get('Pagination') !== null) {
        // this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));

      }
      // return this.paginatedResult;
      return paginatedResult;

    })
  );
}

////get<Member[]>(this.baseUrl//only need to specify the type in our request to tell it what we r receiving back from the server

// private getPaginationHeaders(pageNumber: number, pageSize: number)// for easy to access 

export function getPaginationHeaders(pageNumber: number, pageSize: number)
{// what we going to reutnr from this is the Params
let params = new HttpParams(); // ability toserialize our parameters & thi is going to take care of adding this on to our query string

// //bcoz we r initialzing the page number and a page size we dont need to check to see if we 
// if (page !== null && itemsPerPage !== null) 
    

// {//otherwise we r just goingt o stick with the default & let the server return what he wnat to 
    //  params = params.append('pageNumber', page.toString()); //('pageNumber')// this is our query string 
    params = params.append('pageNumber', pageNumber.toString());
//page.toString());//query string is a string we neeed to make th epage .toString()
    //  params = params.append('pageSize', itemsPerPage.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
    }