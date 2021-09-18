//we see how we receive messages we wna to paginated this responce & working on here & im going to go to
//back to the ImessageRepository is goingot work on this particular method 

//Task<PagedList<MessageDto>> GetMessageThread();
//now we want to give the user the opportunity to see their inbox outbox & unread messages insdie here 
//& we also r returning a page list which means we need to delete the responses as well 
//we want pagination so we derive from pagination Params
namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; }// this is goignt ot be our currently logged in user bcoz we r obviously going to get 
        //the messages for the user  it's logged in & then what we r going to have is a  

        public string Container { get; set; } = "Unread"; //set this by deafult to & unread that what we r going to return by default 
        //the unread messages to the user go back to IMessageRepository


        
    }
}