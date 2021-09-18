
using System;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }//we r also gonna do is track the sender Id 
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }

        public int RecipientId { get; set; }

        public string RecipientUsername { get; set; }

        public string RecipientPhotoUrl { get; set; } //So that we can display the image of the user that sent the message

         public string Content { get; set; }// then we have some messages to specific properties so we'll have a string for the contents

         public DateTime? DateRead { get; set; }/// we r gonna make this optional bcoz we want this to be null if the message has not been read 
         //& what we'l also do is have a date time 
         public DateTime MessageSent { get; set; } 
         // this is going to make a our message DTO & what we'l do we'll get bac to out message repository 

         //after fixing te error from IMessage Repository later on ahead back to data folder create the implementation class for our repository
         // so we'll say message repository
        
    }
}