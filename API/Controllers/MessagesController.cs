
//we want this to e authenticated so we're going to add the authorized attributes the top here 
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController //drive from this 
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository,
         IMapper mapper)// we r going to need access to our user repository here add the IUserRepository and call it user repository 
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]// we gonna use this to create a message so what we'll do is we'll say public async task 

        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)// do we need to return anyhting from this ? Yes we should return our messaageDto so that the message
                                                                                                    //<MessageDto>>// what we r returning from this & will say create a message & we'll use our create message 

        {
            var username = User.GetUsername();//1st let go & get our username 

            if (username == createMessageDto.RecipientUsername.ToLower())  //1st add a check to see if our username is equal to the recipient name in the create messageDto 
                return BadRequest("You cannot send message to youself"); // if they r equal //smthing else we r just not going to allow in this application 
                                                                         // need to do here is get hold both of our users in the sender & the recipient as 
                                                                         //we need to populate the message first of all when we create it & we need to return a Dto 
            var sender = await _userRepository.GetUserByUsernameAsync(username);//thats for the sender 
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);// we will also save our recipient 

            if (recipient == null) return NotFound();// if we dont the recipient  we need to check this & we'll see if it null
                                                     //  not found bcoz we could not find the user 

            var message = new Message//we know we r ready to create a new message that will save our message 
            {
                Sender = sender,//want  to do sender is equal to sender the recipient is equal to recipient the
                Recipient = recipient,//want to
                SenderUsername = sender.UserName,//want to
                RecipientUsername = recipient.UserName,//
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message); // then we can go accross to our message repository & we could say at message 
//and in passing this message & what we want to return from this is a message to & also need to bring in IMapper
           if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDto>(message));// what we should do is return to create it reads we did look this 
           //earlier but to keep things moving 7 we do not have a route where we can get an individual message just now to keeo things moving just
           //gona return messageDto from here map from message that we crated if we dont manage to save it then we r going to return a bad request
           return BadRequest("Failed to send message to");

           //& say failed to send 
         
        }// this is the logic we need for our creation od a message saving ti to the database & returning the messageDto to the user now go head 
        // & test this is fucntional or not 


        [HttpGet]//create an endpoint for this //why httpget bcoz everything gonna come up by a query string & we'll say public async task

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]
         MessageParams messageParams)// we r not returning PageList from this  we r returning is IEnumerable of type messageDto
        //bcoz we r adding the pages onto our header 
        {
            messageParams.Username = User.GetUsername();//need to add our username on to this will say 

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, 
            messages.TotalCount, messages.TotalPages);// then we can add our pagination header// we get this from our messages bcoz this is a 
            //type of page list
            // if u r not sure just hove the method AddPaginationHeader // to see what it's asking for & items per page total items total pages
            return messages; // from here we r not doing anyhting else 
        }

        [HttpGet("thread/{username}")]// addd a method in here to go & get the messagesthread // give this a root parameter
        //username of the other user 
        //Remember we've  always got access to the current username inside our controllers//So we only need to know who the other participant in
        //this conversation is 

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
           var currentUsername = User.GetUsername(); //this is the other user what we need current username
           return Ok(await _messageRepository.GetMessageThread(currentUsername, username)); 
           //.GetMessageThread(currentUsername, username)//the other username which means username in this case
        }


        [HttpDelete("{id}")]//method to delete the message//("{id}")] takethe id of the message thatwe actually want todelete from here 
        public async Task<ActionResult> DeleteMessage(int id)//we don't return anything from deletions 
        {
            var username = User.GetUsername();// we gonna need to get a hold of our user 1st we'll save our username

            var message = await _messageRepository.GetMessage(id);// we alsoneed t get hold of the messagethat will save our message = await & we'll get
//we r gonna to get our message & our message needs to contains the sender & the recipient But did we include them ? Did we equally load 
// load our sender & recipient into that message method based on the fact that we've got the error here ? sorry we didn't d that 
// go to the messagerepository
            if (message.Sender.UserName != username && message.Recipient.UserName != username) return Unauthorized();// &then we nned to run some checks add bunch of conditional here to make sure that we r doing what we want 
        // we r saying that if i were to send a username ot the recipient username is not = to the username then this message has nothing//to do with that user so we r gonna return unauthorised 
        // after that we check to see if the message sender .username is eqaul to 
        if (message.Sender.UserName == username) message.SenderDeleted = true;//if it is then gonna say(aftetr the blacket)
        // then we check the other way & say if the message recipient name
        if(message.Recipient.UserName == username) message.RecipientDeleted = true;//if it
        //willc check to see if bot the sender understtod & the recipient have deleted the messages
        if (message.SenderDeleted && message.RecipientDeleted) 
            _messageRepository.DeleteMessage(message);// then we need both users have deleted the message & therefore we can delete
  //it from the server  we;ll get the message reppsitory// after that we can do the 
    if (await _messageRepository.SaveAllAsync()) return Ok();
    // if that doesn't work then we r gonna return a bad request 7 say prob deleting the message'
    return BadRequest("Problem Deleting the message");
    // go back to repository & gonna need to only return messages for the sender or recipient depending on what they're getting if the message hasn't been deleted 
//So if a user who's sent a message deletes that message sent then er dont want to send that back So we se it in their outbox
// so we'll add a extra check inside our repository methods  
        }
    }
}





