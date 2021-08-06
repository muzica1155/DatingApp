import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  validationErrors: string[] = [];//add a class property //[] = []; initialize to empty array



  constructor(private http: HttpClient)//http service so that we can test error response from our api
   { }

  ngOnInit(): void {
  }//series of methods 
  get404Errors() {
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe((response) => {
      console.log(response);
    }, error => {console.log(error);
    })

  }
  get400Errors() {
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe((response) => {
      console.log(response);
    }, error => {console.log(error);
    })

  }
  get500Errors() {
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe((response) => {
      console.log(response);
    }, error => {console.log(error);
    })

  }
  get401Errors() {
    this.http.get(this.baseUrl + 'buggy/auth').subscribe((response) => {
      console.log(response);
    }, error => {console.log(error);
    })

  }
  get400ValidationErrors() {
    this.http.post(this.baseUrl + 'account/register', {}).subscribe((response) => {
      console.log(response);//account/register/error', {})// send up can empty object
    }, error => {console.log(error);
      this.validationErrors = error;// we know what we r throwing back into our response here 
    })

  }

}
