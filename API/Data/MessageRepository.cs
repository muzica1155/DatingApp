// we just initialize this with a few of these methods & we'll also get our data context 
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data

{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext context, IMapper mapper) // add a constractor & wil inject the data context & call it caontext & will initialize
        {
            _mapper = mapper;
            _context = context;
        }
        //now that we've got htis interface in order to access to differnt entities we r gonna need to upadte our data context 
        //

        public void AddGroup(Group group)
        { /// after maing the changing in DBset DBcontext we come here to implement
            _context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);//all we gonna do is just implement the really simple methods 
            // so if that messge will say context & message & then we r just passing the message 

        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);// for deletemessage a
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        // public Task<Group> GetGroupForConnection(string connectionId)
        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups// so we need to return a group from this 
            .Include(c => c.Connections)//include the connection//(c => Connections)//bcoz thi is all related entity
            .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId)) //add a statement to
            .FirstOrDefaultAsync(); //which is gonna return ad a group
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
            .Include(u => u.Sender)
            .Include(u => u.Recipient) //this should resolve our issue 
            .SingleOrDefaultAsync(x => x.Id == id); // give a expression
            
            // .FindAsync(id);// al we r doing is gonna get the message now if we want to get access to the related entities 
            // then we even need to project or we need to equally load the related entities & we r not gonna to project inside here we need to 
            //change the type of thismethod bcoz we cant use include with a findAsync
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
            .Include(x => x.Connections)//(x => x.Connections) so that we also get the groups connections as well & then 
            .FirstOrDefaultAsync(x => x.Name == groupName);//x.Name == )// for the group name 
            //simple queries to go & get our information 
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages// want to create an Iqurable variable as per normal // creating our query
            .OrderByDescending(m => m.MessageSent)// ordering
            
            /// so we r gonna to order them by most recent fast and then we need to specify as wearable
            //optimization query
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider) // after coping projection before the gt message we get error bcoz our message Dto doesn't contain a property for recipientDeleted
            //afte this we make changes in error code below
             //optimization query
            
            .AsQueryable();//setting this up as queryable 
                           //check the we depend  on which container it will depend on which meassage we return 
                           // add switch expression based on the container 
            query = messageParams.Container switch 
            { //query statement
   // deleting message // 
// add a extra condition inside our inbox & outbox methods 

                // "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username),// 1st case we say inbox //(u => u.Recipient.UserName   // check for the u for user 
  // this is our inbox // if we r the recipient of a message & we ahve read it then this is what is going to go back from this 1 

         //optization query
//          "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username
//           && u.RecipientDeleted == false),// deleting message //
// // we r only returing messages that the recipient has not deleted SAME GOES TO OUTBOX
                
//          "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username
//          && u.SenderDeleted == false),//we also need to do exavtly the same thing in the deafult case where teu unread message r going to

// // && u.RecipientDeleted == false && // we r ony onaa returning mesages that have not been deleted not godowto messagethread
//                 _ => query.Where(u => u.Recipient.UserName ==
//                  messageParams.Username && u.RecipientDeleted == false && u.DateRead == null),// then we need the default case the way that we specify the fault inside here is underscore 
            // == messageParams.Username && ),// && we r going to add a && WE R GOING TO CHECK TO SEE IF THE DATEREAD 
             //u.DateRead == null)// which means that they ahve not read the message yet & this going to bew for our deafult case 
            //optization query
            "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username
          && u.RecipientDeleted == false),// deleting message //
// we r only returing messages that the recipient has not deleted SAME GOES TO OUTBOX
                
         "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
         && u.SenderDeleted == false),//we also need to do exavtly the same thing in the deafult case where teu unread message r going to

// && u.RecipientDeleted == false && // we r ony onaa returning mesages that have not been deleted not godowto messagethread
                _ => query.Where(u => u.RecipientUsername ==
                 messageParams.Username && u.RecipientDeleted == false && u.DateRead == null),
                 //optimization query

            };//query statement

            //we wnat to project in here // wew want to return Dtos from this whcih means we need to bring in Imapper into the particular
            //repository 
          
          //optimization query
            // var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);//project down here But what if we project earlier what if we project this query to a DTO then it would not need 
            //optimization query
            //to select so much from the database when it makes these where queries So we try 
            //ProjectTo<MessageDto>()// we need to pass thi si its configuration parameter & we r 
            //going to get that from Imapper 
           
           //optimization query
           return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);//& we return query instead of the messages that we were creating earlier so we have that in place that's checked to see
            // return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
            //optimization query
        }

        

        // public Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId)
        // {
        //     throw new System.NotImplementedException();
        // }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername,
         string recipientUsername)
        {
            //we r going to get the messages for the both sides of the conversation but what also do inside here is we'll take opportunity to 
    //mark any of the messages that have not currently been read & begin to mark them as read 
    //what it does mean is that we r going to need to get the messages in memorry then do smthng with the messages then we r going to have to
    //map to a DTO so we cant work with Dtos to update our database // We gonna need to execute ther request & get it out to a list & then work
    // with the messages inside memory here 
            var messages = await _context.Messages

            //optimization query
        //    .Include(u => u.Sender).ThenInclude(p => p.Photos)//what we r doing we r including the related properties we've got to include for sender & then include for photos same for the recipient
        //    // we need the inof bcoz we r populating our messageDto with the sender details & we also need that so that we can get their main profile image as well 
        //    //So bcoz we r not projectng What we do need to do is eagerly load the photos for the user bcoz we r going to include those as well
        //    //& this is going to give us access to our use the photos obviously 
           
        //    .Include(u => u.Recipient).ThenInclude(p => p.Photos)//recipient bcoz we want to display the user's photo in the message design as well
           //optimization query

           .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false // we r goign to check the recipient username is equal to the current username
             && m.Sender.UserName == recipientUsername//& then wer going && check that the message.sender 
             || m.Recipient.UserName == recipientUsername//||or condition 
             && m.Sender.UserName == currentUsername && m.SenderDeleted == false
           )//m.SenderDeleted == false that takes care of the API now same thing on the client

           .MarkUnreadAsRead(currentUsername)//bugg fixing 
           
           .OrderBy(m => m.MessageSent)//then we r orderingby the message sent & then we r sending this to a list 
           // before we pass this out to us a list we going 
           .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)//this gives us error in next line // && m.Recipient.UserName//says it doesn't have a property of recipients bcoz this is our messageDto now that we r projected into it 
           //bcz the messageDto does contain a recipient username property we can use that 1 instad & error go awat
           //when we set this property Inside the message thread was setting it aftere we've done this & then we get our unread messages & then we mark them with daytime UTC now but this is after all to mappers the network & the only place that we've asked for our date times 
           //to set as UTC was in Automapper
           .ToListAsync();//then we r sending this to a list 
           //& the sender username is equal to teh recipient username vice versa as well 
           //we r going to use this tool er r not going to project out of this bcoz INSIDE to do to take the opportunity to mark the messages
           //as read when a user gets the message & then any messages that have been sent will mark them as read during this process as well 
           
           //.ToListAsync();// this will remove the errors as we r working out this particular MESSAGE just give us the clear space to work
           //in bcoz we need ot put some logic i here to get the correct messages back 

           //optimization query
           //change after messages scroll
        //    var unreadMessages = messages.Where(m => m.DateRead == null //once we have the list then we r going into our unread messages & checking them to see if there's any that r in read & then we r marking them
           //optimization query
        //    && m.RecipientUsername == currentUsername).ToList();
        //    && m.Recipient.UserName == currentUsername).ToList();//need to get a list of list bcoz we r gonna to need to loop ver this & any unread messages
           //change after messages scroll
           //optimization query
           
           //while the recipient is the current username we gonna mark is right 
           //check the read messages //= messages.// take a look insid our messages that we have
           //wanna check the dateRead //.Where(m = m.DateRead) // check if it is null
           //& we wan to check//m.Recipient.UserName == currentUsername)//
         
         //change after messages scroll
        //    if (unreadMessages.Any()) // check inside here
        //    {
        //        foreach (var message in unreadMessages)//& checking them to see if there's any that r in read & then we r marking them
        //        {
        //         //    message.DateRead = DateTime.Now;
        //         message.DateRead = DateTime.UtcNow;//now this doesn't change anything our server still gonna send back the dates in the same format as they were before
        //         //the only thing is that they r gonna to be using UTC TIME , but we still need our client to know about this We still need our dates to have the Z on
        //         //the end of the dates so that our client can work out the correct local time for this we use AutoMapper

        //        }//loop over this & for any unread message that meets these conditions we r gonna mark as read
               
        //        //change unitofwork
        //     //    await _context.SaveChangesAsync();//take opportunity to save these changes to our database 
        //        //change unitofwork
        //    } 
        //change after messages scroll
              
             //optimization query 
             return messages;//no need to mao down here anymore hust return the messages
            //  return _mapper.Map<IEnumerable<MessageDto>>(messages);//then we r going out & mapping this into a messageDto lets go try different & instead of doing the mapping down here let's try doing the projection before we sned it to a list 
             //w e will return our messagesDto as a list 
             //optimization query 
       
       //we get the conversation of the users would them find out if there's any unread messages for the current user that we've 
       //received will mark them as read & then we return the messagesDto then we messageController to implement this 
       
        } //we get our messages or the messages conversation between 2 users &

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        //changes unitofwork

        // public async Task<bool> SaveAllAsync()
        // {
        //     return await _context.SaveChangesAsync() > 0;// greater than zero that we return a boolean from this 
        //     // head back to application service 
        // }

        //chnages unitofwork
    }
}