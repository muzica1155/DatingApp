import { Component, OnInit } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styles: [
  ]
})
export class MemberDetailComponent implements OnInit {
member: Member;
galleryOptions: NgxGalleryOptions[];
// gallery: NgxGalleryImage[];//
  galleryImages: NgxGalleryImage[];
// in our constructor, we went & injected our member service & our energy on it 
  constructor(private memberService: MembersService, private route: ActivatedRoute) //activated root we want to access when a user 
  //clicks on any of these routes then we r going to send up or they r going to use their username 
  //or we use theusername to decide which user this is & we neeed toacces that particular user profile
  // to get that activated routes
  
  { }
//in ngOnInit, // we went out to our Api & we got our member back going on angular already loaded our temple & this doesn't hapen until after the template has already been created by angular 
//when angular created this template angular didn't know that our user was about to exist 
//to avoid error in our console is add a conditionals to see if we have the member before we try display the member '
  ngOnInit(): void { 
    this.loadMembers();// load our member ///every thing is going here is asynchronous everything happens one after the another 
    this.galleryOptions = [//setting our gallery
      { width: '500px', height: '500px', imagePercent: 100,
       thumbnailsColumns:4, imageAnimation: NgxGalleryAnimation.Slide, preview: false }
    ]//this is an arrey we provide an object about different options
    
  }
  getImages(): NgxGalleryImage[] {
    const imageUrls = [];//this is wat we going to return
    for (const photo of this.member.photos) //for loop so we can loop over all of our different member 
    { imageUrls.push({
      small: photo.url,//need to be an object
      medium: photo.url,///if user doesn't have any photo then it doesn't cause an error in our app
      big: photo.url,// just to be safe here bcoz users may not have photos im just going to use the optional chaining parameter in each one of these 
    })}//push the images that we have inside our mrmber photos that we created eariler 
    return imageUrls;
  }//add a method that we can use to get images outside of our member object
  loadMembers() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
      this.member = member;
      this.galleryImages = this.getImages();//timing issues in the console so this is the fix //
 //our late member after we know that we have got the member that when we going to go & set our gallery imagess to
 //we guarantee that we have tha phots before we attempt to load or initialize the gallery images there 
    })//paramMap the map of parameters & the parameter that we r interested in //not going to use by id going to use by username in app-routing.modules.ts 
 //we r going to using username to decide which route we have gone to. so there will be members, lisa ot members /. & we r going to access that username from the root pareameter in app-routing.modules.ts
 
}

}
