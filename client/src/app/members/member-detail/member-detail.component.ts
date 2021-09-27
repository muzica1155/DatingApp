import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router} from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styles: [
  ]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
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
   
user: User;//add property for the user

  // constructor(private memberService: MembersService, private route: ActivatedRoute, 
    constructor(public presence: PresenceService, private route: ActivatedRoute, 
    private messageService: MessageService, private accountService: AccountService, 
    private router: Router) //activated root we want to access when a user & we get access to the query parameters that we r passing that 
  //clicks on any of these routes then we r going to send up or they r going to use their username 
  //or we use theusername to decide which user this is & we neeed toacces that particular user profile
  // to get that activated routes
  {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    //& this gives us the access to our current user so we can make user of that down here

    this.router.routeReuseStrategy.shouldReuseRoute = () => false; ///this is a fix when user see another uaer 2 profile click the notification of user 1 userMessage it takes u to user 1 messages tab with no messages lolz
  // = () => false;//pass this with a function then set it false  Means is if we go back & repeat the same test
  }
 
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
  //this update was made for bug worked 
  this.router.navigate([], {
    relativeTo: this.route,
  queryParams : {tab: tabId}});//this update was made for bug worked when u r in interest or photo tab
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
    // this.loadMessages();//currently we r loading the messages thi is our API call But we r gonna get this from our signal our hub now instead 
     this.messageService.createHubConnection(this.user, this.member.username)//(this.user)// we gonna access user objects inside here so we'll come up to 
     //where we r injecting things & we r injecting things & we r also gonna 
     //bco we r geting the messages now form the message service What we dont need to do is pass down the messages from here to our child
     // component the member messages components open member-detail.componenet templates

     
  } else // we also do disconnects from thr hub if they go away from the messages tab 
  {
    this.messageService.stopHubConnection();//if they change tabs we stop the hub conection
  } // if we r not activating a tab we also need another way to stop the hub connection if they move away from the member details component
  //completely for that we look another 1 of the angular lifecycle methods called early on destroy what to do when this component is destroyed
  // & we implement this interface as well 

}// if we r not activating a tab we also need another way to stop the hub connection if they move away from the member details component
  //completely for that we look another 1 of the angular lifecycle methods called early on destroy what to do when this component is destroyed
  // & we implement this interface as well 

      ngOnDestroy(): void {
                          this.messageService.stopHubConnection();
                   }
    // so we activated the hub when we click on the messages tab & anything else that we do we r gonna stop that hub conection 
   // now when we stp the connection we r effectively destroyed that hub So if we try & excute this 1 & this 1 has already taken plac ethen we r gonna to get a problem
   //in our application & it will be the normal exception where we r trying to execute smthing on smthing that doesn't exist'
   // what we will do to avoid that situation  from happening bcoz & the reason it could happen is if smbody is on a message tab they create 
   //the connection they go to about page we stop the hub connection they then go to another part of our app 
   // we then try & execute this bcoz we've just destroyed our campaign & then we get the prblem then we go to message.service 
}











