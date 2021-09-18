import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute} from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styles: [
  ]
})
export class MemberDetailComponent implements OnInit {
@ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;// use the selector & call it membertab we can call anything specifically we r gonna get a tabset component here
//this is peovided by ngx bootstrap we gonna give it a type of tabset component  // we also gonna add anothere property
member: Member;
galleryOptions: NgxGalleryOptions[];
// gallery: NgxGalleryImage[];//
galleryImages: NgxGalleryImage[];
// in our constructor, we went & injected our member service & our energy on it 

activeTab: TabDirective;//so each 1 of our temps inside here we 've got a tp directive specifically for as well & what 

messages: Message[] = [];// creata a property for messages thses r gonna be a type of message array & what we'll do is we'll initialize disarray to 
//an empty array & i cant remember if we did this on the other 1 but we need to if we haven't lets check  
// if we dont initialize the array then for sure we r gonna get an error if we try & access a length of the property on smthing 

  constructor(private memberService: MembersService, private route: ActivatedRoute, 
    private messageService: MessageService) //activated root we want to access when a user & we get access to the query parameters that we r passing that 
  //clicks on any of these routes then we r going to send up or they r going to use their username 
  //or we use theusername to decide which user this is & we neeed toacces that particular user profile
  // to get that activated routes
  
  { }
//in ngOnInit, // we went out to our Api & we got our member back going on angular already loaded our temple & this doesn't hapen until after the template has already been created by angular 
//when angular created this template angular didn't know that our user was about to exist 
//to avoid error in our console is add a conditionals to see if we have the member before we try display the member '
  ngOnInit(): void { 
    // this.loadMembers();// load our member ///every thing is going here is asynchronous everything happens one after the another 

    this.route.data.subscribe(data => {
      this.member = data.member; //now we guatantee that our route is gonna have this member inside it what we can also do then ?
     
    }) // we will go & get the membner from route// how we get the data we use the data property this gives us the resolved
    // data of this route 

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);//we use ternary operator & efectively if we have smthing in our query parameters dot tab gonna say
    //if not (params.tan) : this/we r gonna selct the first tab available inside here we r using our select tab method to activate the specific tab & then we can go check to see if this is working 
    //
    })

    this.galleryOptions = [//setting our gallery
      { width: '500px', height: '500px', imagePercent: 100,
       thumbnailsColumns:4, imageAnimation: NgxGalleryAnimation.Slide, preview: false }
    ]//this is an arrey we provide an object about different options

    this.galleryImages = this.getImages(); // paste this ,method to go & get the images 
    
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


//   loadMembers() {  // commented bcoz we r not using after route resolver 
//     this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member => {
//       this.member = member;
      
//       //after route resolver
//       //Inside our loadMember we dont need to call to this gallery img inside here so cut & paste it in OnInit

//       // this.galleryImages = this.getImages();//timing issues in the console so this is the fix //
//  //our late member after we know that we have got the member that when we going to go & set our gallery imagess to
//  //we guarantee that we have tha phots before we attempt to load or initialize the gallery images there 
//     })//paramMap the map of parameters & the parameter that we r interested in //not going to use by id going to use by username in app-routing.modules.ts 
//  //we r going to using username to decide which route we have gone to. so there will be members, lisa ot members /. & we r going to access that username from the root pareameter in app-routing.modules.ts
// }

// copy 7 pasted from member-messages.component.ts
loadMessages() 
{
  //.messageService.GetMessageThread//to bring that we also crate a property insdie here for the messages
  //GetMessageThread(this.username).subscribe(messages// get our username from the member username
  // this.messageService.GetMessageThread(this.username).subscribe(messages => 
  this.messageService.GetMessageThread(this.member.username).subscribe(messages => 
    {
      this.messages = messages;
  })
  
}

selectTab(tabId: number) 
{
 this.memberTabs.tabs[tabId].active = true;

}

onTabActivated(data: TabDirective) // & going to receive the data 7 te data is a type og type directive 
// we want to do is say that list active 
{
  this.activeTab = data;
  // then we have accesss to the info inside that tab 

  // what we can do
  // if (this.activeTab.heading === 'Messages') // we r gonna access the heading property inside the tab for our messages this is the heading //<tab heading= 'Messages'>//
  //that what we can add compare to // we gonna bring the functionality for the loading of the messages indetail component so that know if 
  // that tabs been clicked  bcoZ we know that the messages r gonna to be loaded from this component anyway bcoz the members is a child 
  //component whatwe'l do that functionality inside here going to // member-messages.component.ts cut the loadMessages method
  if (this.activeTab.heading === 'Messages' && this.messages.length === 0)// add another conditional inside here & ew'll say doule && and we'll check to see if
  //they stop messages = 0 bcoz if they're switching between the tabs of a user & we already have messages loaded insie this component
  // then obviously we r not gonna dispose of them & then reload them again just use the same ones
  {
    this.loadMessages();

  }

}

}
