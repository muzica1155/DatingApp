using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = 
        new Dictionary<string, List<string>>();// we r gonna create a dictionary here
        //dictionary store's key & value pairs say the key that we gonna to use is gonna to be string of the user's username and
        // everytime user connects to the hub they r gonna to be given a connection ID Now there is nothing to stop a user from connecting
        //to the same application from a different device & they would get a different connection ID for each different connection
        //that they're having or making to our application //what we will do in the presencetracker is we'll store a list of the connection is a string
        //
         public Task<bool> UserConnected(string username, string connectionId)// we return boolean from this
        // we create couple of methods to add a user to the dictionary when they connect along with their connection ID & also handle the
        //occasion when they disconnect 
        {     
            bool isOnline = false;//add a boolean
            // here we need to be careful here bcoz this dictionary is gona be shared amoungst every1 who connects to our server & the dictionary
            //is not a threat to safe resource So if we had concurrent users trying to update this at the same time then we r probably gonna run into problems 
            lock (OnlineUsers)//we ned to lock the dictionary we use the lock key word 
            {
                //so we r effectively locking the dictionary until we've finished doing what we r doing inside here & what we want to check to see
                //if that we already have a dictionary element with a key of the currently locked in username & IF IT IS then we r gonna add the connectionId
                //otherwise we r gonna to create a new dictionary entry for this particular username with that connectionID 

                if (OnlineUsers.ContainsKey(username))//already got a dictionary entry for the username we wont do anything just keep this online as false 
                //check to see if online users 7 we r checking to see it it;s got a key & key is the username
            // we r gonna chekc to see if we already have such a thing 
                    {
                         
                         OnlineUsers[username].Add(connectionId);//& if we do what we gonna do is goona say we can access the dictionary element with the key of the username 
                    //[username].Add// if we do have 1 we gonna add connectionId to the list
                     // what we r doing jer is for the list the 2nd part we r adding to that list of a new connectionID now if we dont currently
                     //have a user 

                     }
                     else //if we dont currently have a user elements at that dictionary then what will say is online users dont ads & specify the username 
                     {
                         OnlineUsers.Add(username, new List<string>{connectionId});// but if they genuinely just connected & it's a new connection then what we'll do is we'll set this online = true
                        isOnline = true;
                     }
            }
            //    return Task.CompletedTask;//& then we need to return from this outside of the lock 
            return Task.FromResult(isOnline);

        }//that takes care of when uses conected After this when user is disconnected 

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;//
            lock(OnlineUsers)//lock the dictionary
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);//if the user doesn't have a key in the dictionary then we r just gonnato save button task & then we'll
                // if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;
                //inside we check to see if we have a dictionary element 
                //with the key of the currently locked in username then check to see if this key exists 
                //& if it doesn't then we return task to complete a task as we have no more work to do here

               OnlineUsers[username].Remove(connectionId); // if we move past this & what we'll do is we'll say onlineusers bcoz we know we've got this dictionary element 
               //with the key of username & will username & we'll say remove & that will pass in the connectionId & what we'll do we'll check to see
               if (OnlineUsers[username].Count == 0)//if the online users with 
               {
                   OnlineUsers.Remove(username);//then if it is then we'll say online uses & will remove the elemnet with the key of username 
                   isOffline = true;
                     //also want to do that the users only offline if they dont have any connections for that particular username 
                //So if we r removing the dictionary entry for that user then we r gonna set the is offline & 
               }
            }
            //   return Task.CompletedTask;
            return Task.FromResult(isOffline);

        }//& what we also need inside here is a method to get all of the users that r currently connected for that add a 3rd method in here
        public Task<string[]> GetOnlineUsers()
        //just gonna return am array of the usernames from this//Task<string[]>//just 
        {
            string[] onlineUsers;//create a variable to store this in so we;ll say string just call it onlineUsers 
            lock(OnlineUsers)// then we'll lock the dictionary again & we'll say online users 
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();//& inside here what we'll do onlineusrs = & will say online users to access the dictionary order By the key
            //& each key is a user's username & then not gonna select & we r gona select the key then select the key we r not interested in values 
            //we r not interested in the connectionId  so we slect the key then toArray this particular request we r not using our database here 
            //this is all happening in memory & our dictionary is gonna be stored in memory & not the database      
               }
            //if a user;s coneccted from 1 of their devices then we r gonna to say they r online 
             return Task.FromResult(onlineUsers); // return
             //now bcoz this distionary this presents tracker is gonna to be a service that we create & we want this to be shared amongst 
             //every single connection that comes into our server now go to our application service extensions 

        }

        //notification message when user is online 
        public Task<List<string>> GetConnectionsForUser(string username)//we gonna create method that we can get a list of connections for a particular user that stored inside here
        {
            List<string> connectionIds;//bcoz we r inside our presence tracker we r gonna need to lock our dictionary once again 
            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);// GetValueOrDefault// the default for a list it is if the use it doesn't exist in our connections
                //then it's simply gonna to be null that the default that we would be returning if we dont have the value in our dictionary
                
            }//if we have a dictionary element with a username then this is gonna return the list of connectionIds for that particular user 
             
              return Task.FromResult(connectionIds);// there is smthing missing see in messageHUb we could only send the messages to users 
              //or notification to users that r connected to this particular hub  & the idea of this if the user is not connected to this hub or
              //this group then we want to send them a notification how to do ?
              // we can get access to another hub context from which anywhere in our application that will use it inside our other hub the message hub 
              //

        }


    
    }
}