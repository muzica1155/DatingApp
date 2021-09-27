using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker) //pass in the presenceTracker & call 
        {
            _tracker = tracker;
        }

        public override async Task OnConnectedAsync()
        {
        //    await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);//when user connects what we do inject our storage into this or our presence tracker into this 
        //UserConnected();//we need to pass them both the connectionId & the username here or the username 1st 
           var isOnline = await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId); //isOnline = then we get the result back from our user connected after this if statement
           if(isOnline)//then we r gonna send theuser us online to all the connected clients apart from the caller so we r send the next next which is others very similar thing for on disconnected
           await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());//we only return this if the user is online 

           var currentUsers = await _tracker.GetOnlineUsers();//we'll get a list of the currently online users who will save our current users 
           await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);//we r sending back to the caller same thing when user is disconnected
        //    await Clients.All.SendAsync("GetOnlineUsers", currentUsers);//we r returning the list of online users to everybody every single time Clearly that's not really optimal we only want to send this particular list if the user is connecting whereas if they r already connected then really we just want to update the list with who has connected 
           // we gonna send this to everyone that's connected & we r going to have a method called get 
  // when a client connects we r gonna to update our presence tracker & we r gonna to send the updated list of current users back to everyone that's connected &
  //after this disconnected method 
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {  
            var isOffline = await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);//IsOffline = bcoz we r getting that back from our presencetracker
            // await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);//pass in the context.user
            if(isOffline)//then we'll notify the other users that someone is genuinely gone offine but if they r still maintaining 1 connection on a differnt device then they r still gonna be showing us online which is fine 
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
          // then we repeat our behaviour up here to return the list of the currently online 

        //     var currentUsers = await _tracker.GetOnlineUsers();////we r returning the list of online users to everybody every single time when user is disconnected
        //    await Clients.All.SendAsync("GetOnlineUsers", currentUsers); ///removed this bcoz we r gonna to do is just return the updated user when a user is online & will also know who's remov themselves from the connection as well no need the 2nd method go back to presence tracker 
           //& with this info now what we can do next display the currently online user in our application

            await base.OnDisconnectedAsync(exception);

        }

    }//thereis no way to find out whi is connected inside here microsoft did not implement this functionality for a very specific reason
    // & that bcoz if we were in a web firm & we had more than web server we would have no way of getting the connection info from the other
    //server tis service is confined to the server that it's running on So there's a number of strategies for this the most scalable 1 would 
    //be smthing like redics where u store the tracking info in a database like that which can be distributed amoungst many many different servers
    //& we r not to implement that version But what we do is we'll crate a class that's gona to track who has collected & stored that in
    // a dictionary now this is not scalable what I'm about to show u here but work on a single server but it will not work on multiple
    //servers bcoz what we r gonna crate is single server & to scale this onto multiple servers that u would need to use a service such as 
    //Reedus or u could also use ur database to store this info we'll take a look at that approach a bit later 
    // now we do crate a class inside signalr Presencetracker
}