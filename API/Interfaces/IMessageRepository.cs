//define methods that we r going to support inside here 
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IMessageRepository
    {

        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);// task that returns the connection to
        Task<Group> GetMessageGroup(string groupName);
        // not complicated methods but these will allow us to manage our connections to signalR Go across to messageRepository
        Task<Group> GetGroupForConnection(string connectionId);//add new method to optimization message
        void AddMessage(Message message);//add method to add a message to say void & message 
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);// add a task so we can get an individual message & will say 

        // Task<PagedList<MessageDto>> GetMessageForUser();//then ew return a task & for this 1 more user PagedList of type message 
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);// head back to messageRepository
        //
        // Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);// if we got the parameter here to get the messages
        //using the current userID & the recipient id 

        // change of plan 
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername); 
        //change tacking the message// we r gonna add some methods inside here to allow us to manage our groups 

        // instead of using the recipient ID & the user ID we change this //(int currentUserId, int recipientId);// to the string
        //bcoz we r using that for everything up to now & the consistency will we'll take the recipients username as a parameter in the controller
        //& obviously we 've always got access to the current username via the context of a controller //head over messagerepository 
        Task<bool> SaveAllAsync(); //& finally lets add the task of a boolean & will implement the save all async inside here 
        //now create a MessageDto before we go in at the implementation 
    }
}