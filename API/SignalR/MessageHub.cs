// so the concept is that we an create a group for each user we need to define the group name Now the group name in our case is gonna be
// the combination of the username & username but we want this in alphabetical order as well so that whatever a user connects to this particular hub
// we r gonna put them into a group & we want to make sure it's the same group group everytime if they r still chatting to the same user
//So if we've got a group name of lisa todd & todd joins the group we want it still to be lisa & todd 
//So we r gonna to sort that into alphabetical order 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class MessageHub : Hub// drive from the hub class
    {
        //changes unitofwork
        // private readonly IMessageRepository _messageRepository;
        //changes unitofwork

        private readonly IMapper _mapper;

         //changes unitofwork
        // private readonly IUserRepository _userRepository;
         //changes unitofwork

        private readonly IHubContext<PresenceHub> _presenceHub;//we got access to our presence hub from within our messageHUb & we've
        //got a way to find out if the user is online go back to sendmessage method where we r checking to see if the user is in the same group 

        private readonly PresenceTracker _tracker;

        private readonly IUnitOfWork _unitOfWork;//changes unitofwork

         //changes unitofwork
         public MessageHub(IUnitOfWork unitOfWork,
        // public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository,
         //changes unitofwork
         
         IMapper mapper, IHubContext<PresenceHub> presenceHub,
         PresenceTracker tracker)///wee r going to need acces to 2 things at the moment accesss to our message repository
        //& also need access to the IMapper & when we send a message we r gonna have to map it into a DTO & we'll just bring in the API interface
        //for the message repository
        {
            _tracker = tracker;
            _presenceHub = presenceHub;
            //changes unitofwork
            // _userRepository = userRepository;
            //  _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            //changes unitofwork
            _mapper = mapper;
           
        }

        //over here we radd user to a group when we disconnect signal without us needing to do anything is removing the user from the group 
        //But we have no way of knowing who is inside the group any1 given time just using a hub & the reason is same as the reason for not being able to do this with the online presence is that 
        //if we had more than 1 serve then signalR has got no way of knowing if a user's connected to a diffrent server so we have to do this ourselves 
        //& we do a PLAN was to use the presence tracker orginal plan was to add another dictionary But we've got more to track Instead of just a username & a list of connections we've also got an extra level & we also need to track the group name to users
        //& the connections what we do this time look differnt way to tracking users in groups is we'll use our database & this is a perfectly viable solution as well 
        // the optimal solution I'VE mentioned before is to not do this in a database bcoz the database is presistent storage where as smthing like Redus operates in memory on different servers & it could be distributed across differnt servers as well
        //that would be the optimal solution What we r gonna to take a look at is using our database to store the groups bcoz we have it ,
        //its available & it's smthing that we can do relatively easily 
        public override async Task OnConnectedAsync()
        // once again override the unconnectedasync method reveals a public override async task & we r gonna say unconnected async
        {

            var httpContext = Context.GetHttpContext();//what we do get a hold on http context bcoz we need to get hold of a user's username and
            var otherUser = httpContext.Request.Query["user"].ToString();//add a property or a variable for the other user we pass the http context then request query parameters
                                                                         // we gonna access the user  & this is gonna be others username 7 we r gonna pass it to a string 
                                                                         //what this means is that when we make a connection to this hub we r gonaa pass in the other username with the key of user 
                                                                         //& get this into particular property we need to know which user profile the currently logged in user has clicked on & we can get that 
                                                                         //via a query string that we can use when we create this particular hub connection 
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);//what we'll need then is to get the group name & as i memthioned the group name is gonna to need to be a 
                                                                                //combination of both the usernames in alphabetical order 
                                                                                //So then what we'll do is save our group name = get group name & we'll pass in context dot user
                                                                                //thats gonna give us group name thi is gonna be group that a pair of users r gonna belong to Now it doesnt matter if there 's any1 user in tha group connected or by users r connected
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);//A user is always gonna go into this group & theway we do that is// we need to pass this both connectionID
                                                                          //& the string of the group  name So we get our connection ID from the context & we r gonna say...than pass in the group name as the group 
                                                                          //the user is gonna to go into When a user joins th group what we r gonna to do is say...tha
                                                                          //tacking the message time changes 
            // await AddToGroup(Context, groupName);
            var group = await AddToGroup(groupName);//bcoz group is updaed at htis point 
            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);//create a new method 
            
            //change unitofwork
            var messages = await _unitOfWork.MessageRepository.//we r gonna get our messages  if any of the unread messages have been
            //checked inside there to day update to date read then what we'll go is we'll say if & we'll say unit what we do
            // var messages = await _messageRepository.
             //change unitofwork

            GetMessageThread(Context.User.GetUsername(), otherUser);
           //changes unitofwork
            if(_unitOfWork.HasChanges()) await _unitOfWork.Complete();//if any of the unread messages have been
            //checked inside there to day update to date read then what we'll go is we'll say if & we'll say unit what we do we r still gettting our messages
            //inside our message repository If they haven't been read by the recipient then we r gonna Mark them as read Inside there But we r
            //coming back here to check to see if there's any changes & if there r then we r gonna save the changes back to the database 
            //now we've got the units of work percent implemented 

            // await Clients.Group(groupName).SendAsync("ReceiveMessageThread", message);//when client connects then we r sending our message thread to both of our connected users even through 1 will already havr it ?but we have to correct this 
            // pass in the group name
                                                                                      //now this isn't optimal bcoz we r gonna to send the message straignt to bith users even if 1 of the user already has the message right 
                                                                                      //dont worry about that for noe bcoz our goal at the moment is to keep thing simple & start daling optimization later 
                                                                                      //("ReceiveMessageThread",// sending the message threads to the users
              
              await Clients.Caller.SendAsync("ReceiveMessageThread", messages);// so whoever connecting needs to receive message thread they need that particular method
        }
        public override async Task OnDisconnectedAsync(Exception exception)//create the other method & say ..
        {
            // changes After tracking message
            // await RemoveFromMessageGroup(Context.ConnectionId);
             var group = await RemoveFromMessageGroup();//we get this back when a client disconnects now & we dont need to pass in the connectionID
             await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);//both of these methods r now gonna to return an updated group to any1 else thats still connected in that group 
             //Obviously if the group is empty signal doesn't send anything bcoz there's nothing to listen to what it;s sending So we dont need to worry about that SignalR is gonna if the group is mepty it simply doesn't send anything to any1 bcoz the group is empty '
            await base.OnDisconnectedAsync(exception);// all we r gonna do in this now & this needs to be on 
                                                      //now signalR when a suer connects & disconnect & they r a member of the group then when they disconnect is automatically gonna remove them 
                                                      //from that particular group & that 1 of the advantages of using a new hub so that when we do make a connection to this particular 
                                                      //hub then we have access to these 2 methods that we can override But we also need to do bcoz we r creating a new hub is we need to head 
                                                      //over to our starter class 

        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUsername();//1st let go & get our username 

            if (username == createMessageDto.RecipientUsername.ToLower()) // checking if the recipient is trying to send a message to themselves instead of
                                                                          //returning a bad request 
                throw new HubException("You cannot send message to youself"); // we do that istead of bad request 

            //                 return BadRequest("You cannot send message to youself"); 
            // //In signalR we dont have access to API response bcoz this is an Http response we didn't have access to that inside smthing that doesn't use 
            //             //HTTP

            //change unitofwork
             var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            // var sender = await _userRepository.GetUserByUsernameAsync(username);//thats for the sender //go & get our users sender & recipient from our repository
            //change unitofwork
            
            // we r still gonn need to get access to the users so that we can format'sthe message response 
            
            //change unitofwork
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);
            // var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);// we will also save our recipient 
            //change unitofwork

            if (recipient == null) throw new HubException("Not found user");
            var message = new Message
            {
                Sender = sender,//want  to do sender is equal to sender the recipient is equal to recipient the
                Recipient = recipient,//want to
                SenderUsername = sender.UserName,//want to
                RecipientUsername = recipient.UserName,//
                Content = createMessageDto.Content

            };
            var groupName = GetGroupName(sender.UserName, recipient.UserName);//we can move get group name will move this up of the if statement bcoz 
            // we need access to it earlier on So we r gonna get a group anme & then what we can do chanhe this to group name 
            
             //change unitofwork
             var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            // var group = await _messageRepository.GetMessageGroup(groupName);
             //change unitofwork
            
            if (group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;//UtcNow;// then we r gonna ned to change a few other methods to use this for reasons 
                                                   //coz we r gonna have a mix matxh of differnt time zones etc based on the way that dotnet returns the dates 
                                                   // but now we've got a mismatch between our dates 
            }
            else
            {
                var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
                if(connections != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived",
                    new {username = sender.UserName, knownAs = sender.KnownAs});//then we know the users online but we also know they're not part of the same message group then what we can say

                //_presenceHub// we use our presencHub bcoz if they've got a connection there then we know they r connected & we can say client & then 
                //we say clinet again bcoz this can be used to invoke methods on the specified client connection so for all of the connectionIds that
                //a user ,ight have where they r connected to the presence hub then we r gonna use that & pass in the connections
                //("NewMessageReceived"//as the name of this message & then we add anonymous object to create a new anonymous object we just open empty curly bracket
                //gonna say that what we r gonna do when the users online but not connected to the same group & then head back to the client presence.service.ts
                }
            }


             //change unitofwork
             _unitOfWork.MessageRepository.AddMessage(message);
            // _messageRepository.AddMessage(message); // we r still gonna add the message to our repository & we still need to save the message to our repository
             //change unitofwork
            
            //change unitofwork
            if (await _unitOfWork.Complete())
            // if (await _messageRepository.SaveAllAsync()) ///save our changes and return the message to the clinet 
            //change unitofwork
            
            {
                // var group = GetGroupName(sender.UserName, recipient.UserName);//to get the group name & then what we can do instead of returning Ok what we can do saying
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
                // await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));// NewMessage as the method name 
                //"NewMessage"// receiving a new message from our hub what we need to do inside our client is update our message thread observable 
                //to show this new message when it is received // we start in message.service

            }
            //bcoz we r no longer returning from this we do not need the final line here & we'll just
            //thi is our send message in place very similar to what we had done before But we've just refactored it for our signal our hub but we'll take a look at next si the client


        }
        //gonna create a couple of private methods that's gonna handle adding & removing a user from a group 
        // private async Task<bool> AddToGroup(HubCallerContext context, string groupName)// we dont need HubCallerContext inside here my plan was to put this in separate files & we needed the context but bcoz we r inside the hub we dont actually need the hub collar context in here 
        //HubCallerContext) which gives us access to our current username and also 
         //the connectionID 
         private async Task<Group> AddToGroup(string groupName)//return group the message group that a user belongs to & we r gonna send the group back to the Members of the group say they r always 
         //gonna knwo who is connected inside the group that they r in & therefore we'll be able to check to see if the recipient has joined the group & mark the messages as read 
         
        {
            //change unitofwork
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            // var group = await _messageRepository.GetMessageGroup(groupName);// we r already got the group that we r getting from our repositoryanyway 
            //change unitofwork
            
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());// get a new connection when a user connects to this hub thet're always given a new connectionId unless they're reconnecting 
                                                                                              //we do not need the HubCallerContext here as we r still inside the hub

            if (group == null)// check to see if the group is equal to null 
            {
                group = new Group(groupName);//if it is 
                
                //change unitofwork
                _unitOfWork.MessageRepository.AddGroup(group);
                // _messageRepository.AddGroup(group);//then we access our message repository 
                //change unitofwork

            }

            group.Connections.Add(connection);

            //change unitofwork
             if (await _unitOfWork.Complete()) return group;
            // if (await _messageRepository.SaveAllAsync()) return group;//check to make sure that we have saved successfully then return 
            //change unitofwork
            
            //also need to do bcoz we r returning from this now is we need to throw an exception if that doesn't work 
            throw new HubException("Failed to join group");

            // return await _messageRepository.SaveAllAsync();
            //& this method returns a boolean which matches what we r doing up here & we'll do is 

        }

        // private async Task RemoveFromMessageGroup(string connectionId)// we wont anything from this 
         // also we can get the connectionId from the Context this is clearned up later 

        //  private async Task RemoveFromMessageGroup()
        private async Task<Group> RemoveFromMessageGroup()//but what we need is a methd so that we can get the group for this specific connection  instead of just a connection 
        // we need the group & then we can get the connection from inside the group need to go to IMessageRepository
        {
            // var connection = await _messageRepository.GetConnection(connectionId);// we r still inside the hub so we can swap this with connectionId
            //  var connection = await _messageRepository.GetConnection(Context.ConnectionId);// we do need to get the group for this particular connection bcoz we were again gonna want to return the group from this particular method so we add the group inside here
           
           //change unitofwork
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            // var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);//but still we need access to the connection so that we can actually remove the connection 
           //change unitofwork

            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);// we got our group that include our connection we can get it inside there
           
           //change unitofwork
           _unitOfWork.MessageRepository.RemoveConnection(connection);
            // _messageRepository.RemoveConnection(connection);//then we can use the message repository remove connection method 
           //change unitofwork

            // await _messageRepository.SaveAllAsync(); 
            //now we got smthing to do when we connect to the user or new user connects or disconnects form this message hub go to onConnectedAsync method 
            
            //change unitofwork
            if (await _unitOfWork.Complete()) return group;
            // if (await _messageRepository.SaveAllAsync()) return group;// check to see ifthis is successfully
            //change unitofwork
            
            throw new HubException("Failed to remove from group");// if that fails 
            // now we've got 2 methods that return the current populated group that we can use to return to the group themselves 
        }



        private string GetGroupName(string caller, string other)//we create a private helper method & gonna to return a string & get groupname
        //in parameter take in the string of the caller username so we'll just say string caller & string , string other for the other user
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0; //CompareOrdinal method here where we can pass it to strings & then it's gonna give us a return value of even minus 1
            //0 or plus smthing & WE USE THIS TO COMPARE the 2 strings & will pass in caller & the other then check to see if it's minus zero
            //bcoz they compare ordinal returns to us a vakue less than 0 if string is less than string B. 0 if sting & B r = or > 0 string A
            //is > than string B so we r just gonna check to see if the check to see if the caller string A is less than string B, the other.
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";// just return a ternary here & will say return string compar & that will use ternary  then we'll use the string formatters
                                                                             //we'll use a dollar & then inside here what we'll do is we'll specify caller & then we;ll say - & then in the other ....
                                                                             //if it not the case the other case then what we'll do is once again use te string formatter with dollar ....
                                                                             //& this is gonna to ensure that our group name is always gona be alphabetical order for both the caller & the other

        }
    }
}