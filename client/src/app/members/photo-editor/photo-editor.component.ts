import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;//we r going to receive our member from the parent component 
    uploader: FileUploader;//property called upload
     hasBaseDropZoneOver = false;//property
       baseUrl = environment.apiUrl;// need url of the API
        user: User;// add property for our user 

  constructor(private accountService: AccountService, private memberService: MembersService) {
     this.accountService.currentUser$.pipe(take(1)).subscribe
     (user => this.user = user);//inside the contructor here we ll need to get our user out of the observables so what we do 
   }

  ngOnInit(): void {
    this.initializedUploader(); //and then in our energy or in it we can say listo inti
  }
    fileOverBase(e: any) {
      this.hasBaseDropZoneOver = e;
    }// we need to provide this with a method so that we can set our drop zone inside the templete & what to do for this is we need to provide a method called file over base
  // this takes an event of type any
  //thats the configuration we also needto specify what we wnat to do after the iplaod hassuccessfully
    
    setMainPhoto(photo: Photo)//need to provide a set main photo method inside here //we hav got the ellipses there we shoukd tell this that htis is a type of photo.
    //those little lapse r quite hard to see but they do give us a warning that smthing not quite so & we do have photo type so we might as well use it 
    //doesn't really matter where we put these methods as long as we r happy with their location ourselves the computer or angular certainly doesn't care about
    { //we will pass in the photo to this method & then we will
      this.memberService.setMainPhoto(photo.id).subscribe(() => {
        this.user.photoUrl = photo.url;
        this.accountService.setCurrentUser(this.user);//(this.user)// which gonna update bot our current user observables & also gonna update our photo inside localStoragebcoz we r also setting that inside 
        //means if we close up browser & then come back its going to be able to get the photo out of what we r storing in there & use that to display on our nav bar 
        this.member.photoUrl = photo.url;
        //& then what we need to do through each of the member photos & switch to one that is main to false & set the photo that we have here to try to see what er can use for that
        this.member.photos.forEach(p => {
          if (p.isMain) p.isMain = false;//cjeck to see if p is the main photo currently//if it is p =
          if (p.id === photo.id) p.isMain = true;//mean equal to true for that one bcoz thats the one we r setting to the main photo.
          
        })
      })
      
    }
    
    deletePhoto(photoId: number) {
      this.memberService.deletePhoto(photoId).subscribe(() => {
        this.member.photos = this.member.photos.filter(x => x.id !== photoId);//this filters out all of the other photos or 
        //more accurately this return an array of all of the phots that r not equal t the photos ID we r 
        //& we also dont need to worry about handling the errors bcoz our intercept is taking care of this for us 

      })
    }

  initializedUploader() //methods 
  { 
      this.uploader = new FileUploader({
         url: this.baseUrl + 'users/add-photo',/// configuration propertiers
         authToken: 'Bearer ' + this.user.token,/// authToken: 'Bearer ' // this means we need to go get our user from our account controller
         isHTML5: true, //taken in her auth bcoz this is not going to be using via our http interceptors this is kind of separate
         allowedFileType: ['image'],
         removeAfterUpload: true,// there is prperty we can set saying remove after upload if we want to remove this from the drop zone after the upload has taken place & we r going 
         autoUpload: false,//make the user a click a  button & we can also set the max file size here 
         maxFileSize: 10 * 1024 * 1024,// set it ot what the maximum that cloud & we will take on a free account which is 10 megabytes

        });//add configuration we need less 
        

        this.uploader.onAfterAddingFile = (file) => {
         file.withCredentials = false;//otherwise we r going to need to make an 
        //  //adjustment to our APi cause configuration and allow credentials to go up with our request/ we dont need to bcoz wqe r using the barrier taken to sned our cendential with this file & then we need to
        
        // }//little bit more configuration to the uploadeer as well 
        // // goint ot target on after adding file 
  }

  this.uploader.onSuccessItem = (item, response, status, headers) => {
   if(response) {//if we have got a response & if we have them 
    const photo = JSON.parse(response);
    this.member.photos.push(photo);//this take care of our components coach 7 we take a look at what we need 

   } //& without the arrow function we check to see 
  }// inside the methos there is required parameters here need to pass all of this 

  }}
