import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
  title: string;//add the properties // jus tlike role modal any anything in our initial state is available as a property in our model when we open it 
  message: string;//
  btnOkText: string;
  btnCancelText: string;
  result: boolean;// create a adiitional property  wer gonna store what ever they selct as the result which is basically true or false 
  //do u wnat to continue or cancel 

  constructor(public bsModalRef: BsModalRef)//bcoz we access this inside our template as well 
   { }//

  ngOnInit(): void {
  }

     confirm()// add couple of method s
     {
       this.result = true;//if they confirm we'll set this results = to true 
       this.bsModalRef.hide();
     }
     decline()//
     {
       this.result = false;
       this.bsModalRef.hide();// after this open templete

     }

}
