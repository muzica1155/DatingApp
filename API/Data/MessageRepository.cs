// we just initialize this with a few of these methods & we'll also get our data context 
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
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

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);//all we gonna do is just implement the really simple methods 
            // so if that messge will say context & message & then we r just passing the message 

        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);// for deletemessage a
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

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _context.Messages// want to create an Iqurable variable as per normal
            .OrderByDescending(m => m.MessageSent)/// so we r gonna to order them by most recent fast and then we need to specify as wearable
            .AsQueryable();//
                           //check the we depend  on which container it will depend on which meassage we return 
                           // add switch expression based on the container 
            query = messageParams.Container switch
            {
   // deleting message // 
// add a extra condition inside our inbox & outbox methods 


                // "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username),// 1st case we say inbox //(u => u.Recipient.UserName   // check for the u for user 
  // this is our inbox // if we r the recipient of a message & we ahve read it then this is what is going to go back from this 1 

         "Inbox" => query.Where(u => u.Recipient.UserName == messageParams.Username
          && u.RecipientDeleted == false),// deleting message //
// we r only returing messages that the recipient has not deleted SAME GOES TO OUTBOX
                
         "Outbox" => query.Where(u => u.Sender.UserName == messageParams.Username
         && u.SenderDeleted == false),//we also need to do exavtly the same thing in the deafult case where teu unread message r going to

// && u.RecipientDeleted == false && // we r ony onaa returning mesages that have not been deleted not godowto messagethread
                _ => query.Where(u => u.Recipient.UserName ==
                 messageParams.Username && u.RecipientDeleted == false && u.DateRead == null),// then we need the default case the way that we specify the fault inside here is underscore 
                                                               // == messageParams.Username && ),// && we r going to add a && WE R GOING TO CHECK TO SEE IF THE DATEREAD 
                                                               //u.DateRead == null)// which means that they ahve not read the message yet & this going to bew for our deafult case 
            };

            //we wnat to project in here // wew want to return Dtos from this whcih means we need to bring in Imapper into the particular
            //repository 

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);//ProjectTo<MessageDto>()// we need to pass thi si its configuration parameter & we r 
            //going to get that from Imapper 

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
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
           .Include(u => u.Sender).ThenInclude(p => p.Photos)//So bcoz we r not projectng What we do need to do is eagerly load the photos for the user bcoz we r going to include those as well
           //& this is going to give us access to our use the photos obviously 
           
           .Include(u => u.Recipient).ThenInclude(p => p.Photos)//recipient bcoz we want to display the user's photo in the message design as well

           .Where(m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false // we r goign to check the recipient username is equal to the current username
             && m.Sender.UserName == recipientUsername//& then wer going && check that the message.sender 
             || m.Recipient.UserName == recipientUsername//||or condition 
             && m.Sender.UserName == currentUsername && m.SenderDeleted == false
           )//m.SenderDeleted == false that takes care of the API now same thing on the client
           .OrderBy(m => m.MessageSent)
           // before we pass this out to us a list we going 
           .ToListAsync();//& the sender username is equal to teh recipient username vice versa as well 
           //we r going to use this tool er r not going to project out of this bcoz INSIDE to do to take the opportunity to mark the messages
           //as read when a user gets the message & then any messages that have been sent will mark them as read during this process as well 
           
           //.ToListAsync();// this will remove the errors as we r working out this particular MESSAGE just give us the clear space to work
           //in bcoz we need ot put some logic i here to get the correct messages back 

           var unreadMessages = messages.Where(m => m.DateRead == null 
           && m.Recipient.UserName == currentUsername).ToList();//need to get a list of list bcoz we r gonna to need to loop ver this & any unread messages
           //while the recipient is the current username we gonna mark is right 
           //check the read messages //= messages.// take a look insid our messages that we have
           //wanna check the dateRead //.Where(m = m.DateRead) // check if it is null
           //& we wan to check//m.Recipient.UserName == currentUsername)//

           if (unreadMessages.Any()) // check inside here
           {
               foreach (var message in unreadMessages)
               {
                   message.DateRead = DateTime.Now;
               }//loop over this & for any unread message that meets these conditions we r gonna mark as read

               await _context.SaveChangesAsync();//take opportunity to save these changes to our database 
           } 
       
             return _mapper.Map<IEnumerable<MessageDto>>(messages);//w e will return our messagesDto as a list 
       //we get the conversation of the users would them find out if there's any unread messages for the current user that we've 
       //received will mark them as read & then we return the messagesDto then we messageController to implement this 
       
        } //we get our messages or the messages conversation between 2 users &

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;// greater than zero that we return a boolean from this // head back to application service 

        }
    }
}