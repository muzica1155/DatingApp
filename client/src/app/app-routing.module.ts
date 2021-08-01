import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  { //when user browsers to localhost for 200 directly then this component will open // HomeComponent// will be loaded
    path: '', component: HomeComponent //each of the roots will be the objects// for our home compoenet going to be an empty string
  },
  {
   path: '',
   runGuardsAndResolvers: 'always', canActivate: [AuthGuard],children: 
   [
    { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]},///
    { path: 'members/:id', component: MemberDetailComponent}, //each members is going to have a root parameter and added a placeholder
    //when we browser to members one or members two, then we r going ro load up the member details components 
    { path: 'lists', component: ListsComponent},
    { path: 'messages', component: MessagesComponent},

   ]
  },
  
  { path: '**', component: HomeComponent, pathMatch: 'full'},//wild card root as in the user's typed in smthing that doesn't match anything inside our reconfiguration
  // & where we'll direct them to for the time being
   // also specify an extra attribute t& we say pathmatch if user doesn't match any of these routes then we r going to redirect them to the home component or load, 

];//use this array to provide the routes tha we tell angular about so 

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
